using Microsoft.Win32;
using practice_hothouse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ClosedXML.Excel;


namespace practice_hothouse.View
{
    public partial class WindowHectareInfo : Window
    {
        private bool isOpenHarvestView = true; // Добавляем поле

        private Hectare h; // Текущий гектар
        DbHothouseContext db = new DbHothouseContext();

        public WindowHectareInfo(Hectare currentHectare)
        {
            InitializeComponent();
            h = currentHectare;

            nameLabel.Content = "Вы выбрали " + h.HectareName + "!";

            LoadData();
        }

        private void LoadData()
        {
            var sowings = (from sowing in db.Sowings
                           join hectare in db.Hectares on sowing.HectareId equals hectare.HectareId
                           join seed in db.Seeds on sowing.SeedId equals seed.SeedId
                           where hectare.HectareId == h.HectareId
                           select new
                           {
                               sowing.SowingId,
                               sowing.SowingDate,
                               sowing.ExpectedYield,
                               sowing.IsHarvestClosed,
                               sowing.NumberInSeason,
                               seed.DaysToFirstHarvest,
                               seed.HarvestFrequency,
                               seed.NumberOfHarvests,
                               SeedType = seed.SeedType,
                               SeedVariety = seed.SeedVariety
                           }).ToList();

            var listViewData = sowings.Select(data =>
            {
                decimal totalYield = db.HarvestHistories
                                       .Where(h => h.SowingId == data.SowingId)
                                       .Sum(h => (decimal?)h.HarvestedAmount) ?? 0;

                return new
                {
                    data.SowingId,
                    data.SeedType,
                    data.SeedVariety,
                    data.NumberInSeason,
                    data.ExpectedYield,
                    TotalYield = totalYield,
                    GreenhouseHarvestDate = CalculateHarvestDate(
                        data.SowingDate.HasValue ? data.SowingDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                        data.DaysToFirstHarvest,
                        data.HarvestFrequency,
                        data.NumberOfHarvests),
                    HarvestCloseStatus = data.IsHarvestClosed == 1 ? "Да" : "Нет",
                    BackgroundColor = (data.ExpectedYield > totalYield) ? "Red" : "Green"
                };
            }).ToList();

            lvOpenHarvest.ItemsSource = listViewData.Where(x => x.HarvestCloseStatus == "Нет").ToList();
            lvClosedHarvest.ItemsSource = listViewData.Where(x => x.HarvestCloseStatus == "Да").ToList();
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            isOpenHarvestView = !isOpenHarvestView;

            if (isOpenHarvestView)
            {
                toggleButton.Content = "Закрытые сборы";
                lvOpenHarvest.Visibility = Visibility.Visible;
                lvClosedHarvest.Visibility = Visibility.Collapsed;
                btnArchive.Visibility = Visibility.Visible; // Показываем кнопку "Архивировать"
                exportButton.Visibility = Visibility.Collapsed; // Скрываем кнопку экспорта
            }
            else
            {
                toggleButton.Content = "Открытые сборы";
                lvOpenHarvest.Visibility = Visibility.Collapsed;
                lvClosedHarvest.Visibility = Visibility.Visible;
                btnArchive.Visibility = Visibility.Collapsed;
                exportButton.Visibility = Visibility.Visible;
            }

            UpdateEditButtonVisibility(); 
        }
        private void UpdateEditButtonVisibility()
        {
            if (App.UserRole == 1 || App.UserRole == 2)
            {
                if (!isOpenHarvestView) 
                {
                    Button_Edit.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Button_Edit.Visibility = Visibility.Visible; 
                }
            }
            else if (App.UserRole == 3)
            {
                Button_Edit.Visibility = Visibility.Visible;
            }
        }


        private DateTime? CalculateHarvestDate(DateTime? sowingDate, int? daysToFirstHarvest, int? harvestFrequency, int? numberOfHarvests)
        {
            if (sowingDate == null || daysToFirstHarvest == null || harvestFrequency == null || numberOfHarvests == null)
                return null;

            return sowingDate.Value
                .AddDays(daysToFirstHarvest.Value)
                .AddDays(harvestFrequency.Value * numberOfHarvests.Value);
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem listViewItem)
            {
                var selectedItem = listViewItem.Content as dynamic;

                if (selectedItem != null)
                {
                    string seedType = selectedItem.SeedType;
                    string seedVariety = selectedItem.SeedVariety;
                    DateTime? greenhouseHarvestDate = selectedItem.GreenhouseHarvestDate;
                    int sowingId = selectedItem.SowingId;
                    var sowingRecord = db.Sowings.FirstOrDefault(x => x.SowingId == sowingId);

                    if (sowingRecord != null)
                    {
                        App.currentSowing = sowingRecord;
                        WindowPlantingInfo windowPlantingInfo = new WindowPlantingInfo(App.currentSowing);
                        windowPlantingInfo.Show();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Запись не найдена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка: Невозможно извлечь данные из элемента.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void MouseLeftButtonDown_profile(object sender, MouseButtonEventArgs e)
        {
            WindowProfile wProfile = new WindowProfile(App.currentUser);
            wProfile.Show();
            Close();
        }
        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            if (lvClosedHarvest.ItemsSource == null)
            {
                MessageBox.Show("Нет данных для экспорта.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                Title = "Сохранить файл Excel",
                FileName = "HectareData.xlsx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    var workbook = new XLWorkbook();
                    var worksheet = workbook.Worksheets.Add("Hectare Data");

                    worksheet.Cell(1, 1).Value = "Тип семян";
                    worksheet.Cell(1, 2).Value = "Сорт семян";
                    worksheet.Cell(1, 3).Value = "Номер посева";
                    worksheet.Cell(1, 4).Value = "Дата последнего сбора";
                    worksheet.Cell(1, 5).Value = "Ожидаемый урожай (кг)";
                    worksheet.Cell(1, 6).Value = "Собрано урожая (кг)";

                    int row = 2;
                    foreach (var item in lvClosedHarvest.Items)
                    {
                        dynamic sowingData = item;

                        worksheet.Cell(row, 1).Value = sowingData.SeedType;
                        worksheet.Cell(row, 2).Value = sowingData.SeedVariety;
                        worksheet.Cell(row, 3).Value = sowingData.NumberInSeason;
                        worksheet.Cell(row, 4).Value = sowingData.GreenhouseHarvestDate?.ToString("dd.MM.yyyy");
                        worksheet.Cell(row, 5).Value = sowingData.ExpectedYield;
                        worksheet.Cell(row, 6).Value = sowingData.TotalYield;

                        row++;
                    }

                    worksheet.Columns().AdjustToContents();
                    workbook.SaveAs(saveFileDialog.FileName);
                    MessageBox.Show("Данные успешно экспортированы в Excel.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при экспорте данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        public void MouseLeftButtonDown_main(object sender, MouseButtonEventArgs e)
        {
            WindowPlots wPlots = new WindowPlots(App.currentUser);
            wPlots.Show();
            Close();
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            WindowHectare wHectare = new WindowHectare(App.currentPlot);
            wHectare.Show();
            Close();
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            WindowAddSowing windowAddSowing = new WindowAddSowing();
            windowAddSowing.Show();
            Close();
        }

        private void Button_Edit_Click(object sender, RoutedEventArgs e)
        {
            dynamic selectedItem = isOpenHarvestView ? lvOpenHarvest.SelectedItem : lvClosedHarvest.SelectedItem;

            if (selectedItem == null)
            {
                MessageBox.Show("Выберите посев для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int sowingId = selectedItem.SowingId;
            var sowingRecord = db.Sowings.FirstOrDefault(s => s.SowingId == sowingId);

            if (sowingRecord != null)
            {
                App.currentSowing = sowingRecord;

                WindowEditSowing editWindow = new WindowEditSowing(App.currentSowing);
                editWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Ошибка: Посев не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void Button_Archive_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = lvOpenHarvest.SelectedItem as dynamic;

            if (selectedItem == null)
            {
                MessageBox.Show("Выберите сбор для закрытия.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int sowingId = selectedItem.SowingId;
            var sowingRecord = db.Sowings.FirstOrDefault(s => s.SowingId == sowingId);

            if (sowingRecord != null)
            {
                sowingRecord.IsHarvestClosed = 1;
                db.SaveChanges();
                LoadData();

                MessageBox.Show("Сбор успешно закрыт.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Ошибка: Сбор не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
