﻿using System;
using System.Collections.Generic;
using Gvin.Injection.Configuration;
using VolumeCorrector.Model.ProgramSettings;
using VolumeCorrector.Model.VolumeCorrection;
using VolumeCorrector.Model.VolumeCorrection.Strategies;
using VolumeCorrector.Presenters;
using VolumeCorrector.Views;

namespace VolumeCorrector
{
    public class InjectorConfiguration : IInjectorConfiguration
    {
        public Dictionary<Type, InjectorMappingType> GetMapping()
        {
            return new Dictionary<Type, InjectorMappingType>
            {
                // Views
                {typeof(IOptionsView), new InjectorMappingType{Type = typeof(FormOptions)}},
                {typeof(INotifyIconView), new InjectorMappingType{Type=typeof(NotifyIconView)}},
                // Presenters
                {typeof(IOptionsPresenter), new InjectorMappingType{Type=typeof(OptionsPresenter)}},
                {typeof(INotifyIconPresenter), new InjectorMappingType{Type=typeof(NotifyIconPresenter)}},
                // Model
                {typeof(IVolumeMonitor), new InjectorMappingType{Object = new VolumeMonitor(new MediumCorrectionStrategy())}},
                {typeof(ISettingsManager), new InjectorMappingType{Object = new SettingsManager()}}
            };
        }
    }
}