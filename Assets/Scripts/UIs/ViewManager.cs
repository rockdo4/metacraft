using System.Collections.Generic;
public class ViewManager : Singleton<ViewManager>
{
    public View startingView;

    private View currentView;

    public List<View> views = new ();

    private readonly Stack<View> history = new ();
    private void Start()
    {   
        foreach (var view in views)
        {
            view.Initialize();
            view.Hide();
        }

        if (startingView != null)
        {
            Show(startingView);
        }
    }
    public static T GetView<T>() where T : View
    {
        foreach (var view in Instance.views)
        {
            if (view is T tView)
            {
                return tView;
            }
        }
        return null;
    }
    public static void Show<T>(bool remember = true) where T : View
    {
        T viewToDisplay = null;  
        var length = Instance.views.Count;    

        for (int i = 0; i < length; ++i)
        {
            if (Instance.views[i] is T tView)
            {
                viewToDisplay = tView;
                break;
            }
            if (i == length - 1)
                return;
        }

        if (Instance.currentView != null && remember)
        {
            Instance.history.Push(Instance.currentView);
        }

        Instance.currentView?.Hide();

        viewToDisplay.Show();

        Instance.currentView = viewToDisplay;
    }
    public static void Show(View view, bool remember = true)
    {
        if (Instance.currentView != null)
        {
            if (remember)
            {
                Instance.history.Push(Instance.currentView);
            }
            Instance.currentView.Hide();
        }
        view.Show();
        Instance.currentView = view;
    }
    public static void ShowLast()
    {
        if (Instance.history.Count != 0)
        {
            Show(Instance.history.Pop(), false);
        }
    }
}