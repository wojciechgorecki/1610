// <copyright file="ViewModelLocator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:MalaUkladnica"
                           x:Key="Locator" />
  </Application.Resources>
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/
namespace SmallStacker.ViewModel
{
    using CommonServiceLocator;
    using GalaSoft.MvvmLight.Ioc;

    // using Microsoft.Practices.ServiceLocation;

    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelLocator"/> class.
        /// </summary>
        ///




        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<ButtonsViewModel>();
            SimpleIoc.Default.Register<DeleteContainerViewModel>();
            SimpleIoc.Default.Register<GetContainerInfoViewModel>();
            SimpleIoc.Default.Register<GetContainerViewModel>();

            SimpleIoc.Default.Register<GetHistoryViewModel>();
            SimpleIoc.Default.Register<LogViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SendContainerViewModel>();

        }

        /// <summary>
        /// Gets TO-DO
        /// </summary>
        /// <value>
        /// TO-DO text
        /// </value>

        public ButtonsViewModel Buttons
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ButtonsViewModel>();
            }

        }

        public DeleteContainerViewModel DeleteContainer
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DeleteContainerViewModel>();
            }

        }

        public GetContainerInfoViewModel GetContainerInfo
        {
            get
            {
                return ServiceLocator.Current.GetInstance<GetContainerInfoViewModel>();
            }

        }

        public GetContainerViewModel GetContainer
        {
            get
            {
                return ServiceLocator.Current.GetInstance<GetContainerViewModel>();
            }

        }

        public GetHistoryViewModel GetHistory
        {
            get
            {
                return ServiceLocator.Current.GetInstance<GetHistoryViewModel>();
            }

        }



        public LogViewModel Log
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LogViewModel>();
            }

        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }

        }
        
        public SendContainerViewModel SendContainer
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SendContainerViewModel>();
            }

        }

        /// <summary>
        /// TO-DO
        /// </summary>
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}