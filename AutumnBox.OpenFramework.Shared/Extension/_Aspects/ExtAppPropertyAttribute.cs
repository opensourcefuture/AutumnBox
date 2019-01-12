﻿/*************************************************
** auth： zsh2401@163.com
** date:  2018/8/13 4:15:02 (UTC +8:00)
** desc： ...
*************************************************/
using AutumnBox.Basic.Device;
using AutumnBox.Basic.Device.Management.AppFx;
using AutumnBox.OpenFramework.Open;

namespace AutumnBox.OpenFramework.Extension
{
    /// <summary>
    /// 声明需要依赖的APP,秋之盒将在运行前进行检查
    /// </summary>
    public sealed class ExtAppPropertyAttribute : BeforeCreatingAspect
    {
        private readonly string value;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="value"></param>
        public ExtAppPropertyAttribute(string value)
        {
            this.value = value;
        }
        /// <summary>
        /// 在那之前...
        /// </summary>
        /// <param name="args"></param>
        /// <param name="canContinue"></param>
        public override void BeforeCreating(BeforeCreatingAspectArgs args, ref bool canContinue)
        {
            IDevice selectDevice = args.Context.GetService<IDeviceSelector>(ServicesNames.DEVICE_SELECTOR).GetCurrent(args.Context);
            if (!InstallApplication(selectDevice, value))
            {
                ChoiceResult choice = ChoiceResult.Deny;
                args.Context.App.RunOnUIThread(() =>
                 {
                     choice = args.Context.Ux.DoChoice("OpenFxInstallAppFirst", "OpenFxInstallBtnIgnore", "OpenFxInstallBtnOk");
                 });
                canContinue = (choice == ChoiceResult.Deny);
            }
        }
        private static bool InstallApplication(IDevice device, string pkgName)
        {
#pragma warning disable CS0618 // 类型或成员已过时
            return new PackageManager(device).IsInstall(pkgName) == true;
#pragma warning restore CS0618 // 类型或成员已过时
        }
    }
}
