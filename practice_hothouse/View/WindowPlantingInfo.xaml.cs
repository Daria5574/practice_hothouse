﻿using practice_hothouse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace practice_hothouse.View
{
    /// <summary>
    /// Логика взаимодействия для WindowPlantingInfo.xaml
    /// </summary>
    public partial class WindowPlantingInfo : Window
    {
        DbHothouseContext db = new DbHothouseContext();
        Sowing s = new Sowing();
        public WindowPlantingInfo(Sowing s)
        {
            InitializeComponent();
            var seedData = (from sow in db.Sowings
                            join seed in db.Seeds on sow.SeedId equals seed.SeedId
                            where sow.SowingId == s.SowingId
                            select new
                            {
                                seed.SeedType,
                                seed.SeedVariety,
                                seed.DaysToFirstHarvest,
                                seed.HarvestFrequency,
                                seed.NumberOfHarvests
                            }).FirstOrDefault();

            DateTime? CalculateHarvestDate(DateOnly? sowingDate, int? daysToFirstHarvest, int? harvestFrequency, int? numberOfHarvests)
            {
                if (sowingDate == null || daysToFirstHarvest == null || harvestFrequency == null || numberOfHarvests == null)
                    return null;

                // Преобразование DateOnly? в DateTime?
                DateTime? sowingDateTime = sowingDate?.ToDateTime(TimeOnly.MinValue);

                return sowingDateTime.Value
                    .AddDays(daysToFirstHarvest.Value)
                    .AddDays(harvestFrequency.Value * numberOfHarvests.Value);
            }

            DateTime? lastHarvestDate = CalculateHarvestDate(
                s.SowingDate,
                seedData.DaysToFirstHarvest,
                seedData.HarvestFrequency,
                seedData.NumberOfHarvests
            );

            // Добавляем 1 день к дате последнего урожая для получения даты уборки
            DateTime? cleanupDate = lastHarvestDate?.AddDays(1);


            titleLabel.Content = seedData.SeedType + " \"" + seedData.SeedVariety + "\" " + s.NumberInSeason + " посев";
            typeLabel.Content = "Тип: " + seedData.SeedType;  
            varietyLabel.Content = "Сорт: " + seedData.SeedVariety;  
            sowingDateLabel.Content = "Дата посева: " + s.SowingDate?.ToString("dd.MM.yyyy");  
            seedlingsCountLabel.Content = "Количество посаженной рассады: " + s.NumberOfPlantedSeeds.ToString(); 
            expectedYieldLabel.Content = "Ожидаемый урожай (кг): " + s.ExpectedYield.ToString();  
            firstHarvestDateLabel.Content = "Планируемая дата первого сбора: " + s.ExpectedYield?.ToString("dd.MM.yyyy");  
            harvestFrequencyLabel.Content = "Периодичность сбора урожая: " + seedData.HarvestFrequency.ToString();  
            harvestCountLabel.Content = "Количество сборов: " + seedData.NumberOfHarvests.ToString();  
            lastHarvestDateLabel.Content = "Планируемая дата последнего урожая: " + lastHarvestDate?.ToString("dd.MM.yyyy"); 
            cleanupDateLabel.Content = "Дата уборки теплицы: " + cleanupDate?.ToString("dd.MM.yyyy");  
            totalYieldLabel.Content = "Всего собрано урожая (кг): " + s.ActualYield.ToString(); 
        }
        public void MouseLeftButtonDown_main(object sender, MouseButtonEventArgs e)
        {
            WindowPlots wPlots = new WindowPlots(App.currentUser);
            wPlots.Show();
            Close();
        }
        private void MouseLeftButtonDown_profile(object sender, MouseButtonEventArgs e)
        {
            WindowProfile wProfile = new WindowProfile(App.currentUser);
            wProfile.Show();
            Close();
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            WindowHectare wHectare = new WindowHectare(App.currentPlot);
            wHectare.Show();
            Close();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void historyButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}