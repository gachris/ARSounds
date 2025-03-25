using ARSounds.UI.Controls;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ARSounds.UI;

// All the code in this file is included in all platforms.
public class PopupAction
{
    public static async Task<T> DisplayPopup<T>(BasePopupPage page) where T : new()
    {
        try
        {
            if (Microsoft.Maui.Controls.Application.Current?.MainPage != null)
            {
                await Microsoft.Maui.Controls.Application.Current.MainPage.Navigation.PushModalAsync(page);
            }
            return (T)await page.returnResultTask.Task;
        }
        catch (Exception)
        {
            return default;
        }
    }

    public static async Task<string> DisplayPopup(BasePopupPage page)
    {
        try
        {
            if (Microsoft.Maui.Controls.Application.Current?.MainPage != null)
                await Microsoft.Maui.Controls.Application.Current.MainPage.Navigation.PushModalAsync(page);

            return (string)await page.returnResultTask.Task;
        }
        catch (Exception)
        {
            return "";
        }
    }

    public static async Task ClosePopup(object returnValue = null)
    {
        if (Microsoft.Maui.Controls.Application.Current?.MainPage != null && Microsoft.Maui.Controls.Application.Current.MainPage.Navigation.ModalStack.Count > 0)
        {
            try
            {
                var currentPage = (BasePopupPage)Microsoft.Maui.Controls.Application.Current.MainPage.Navigation.ModalStack.LastOrDefault();
                currentPage?.returnResultTask.TrySetResult(returnValue);
            }
            catch (Exception)
            {

            }
            await Microsoft.Maui.Controls.Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }

}
