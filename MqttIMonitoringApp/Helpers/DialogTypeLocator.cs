using MvvmDialogs.DialogTypeLocators;
using System;
using System.ComponentModel;

namespace MqttMonitoringApp.Helpers
{
    public class DialogTypeLocator : IDialogTypeLocator
    {
        /// <summary>
        /// 특정 View모델에 Dialog타입을 위치 시키는 메서드
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public Type Locate(INotifyPropertyChanged viewModel)
        {
            Type viewModelType = viewModel.GetType();

            var dialogFullName = viewModelType.FullName;
            dialogFullName = dialogFullName.Substring(0, dialogFullName.Length - "Model".Length);


            return viewModelType.Assembly.GetType(dialogFullName);
        }
    }

}
