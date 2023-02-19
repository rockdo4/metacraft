using System.Collections.Generic;
using UnityEngine;
public class ViewManager : MonoBehaviour
{
    public static ViewManager Instance { get; private set; }

    public View startingView;

    private View currentView;

    public List<View> views = new ();    
    private void Awake() => Instance = this; 
   
    private void Start()
    {   
        foreach (var view in views)
        {            
            view.Hide();
        }

        if (startingView != null)
        {
            Show(startingView);
        }
    } 
    public static void Show(int index)
    {
        if (index >= Instance.views.Count)
            return;

        var viewToDisplay = Instance.views[index];
        if (Instance.currentView != null)
        {       
            Instance.currentView.Hide();
        }
        viewToDisplay.Show();
        Instance.currentView = viewToDisplay;
    }
    public static void Show(View view)
    {
        if (Instance.currentView != null)
        {
            Instance.currentView.Hide();    
        }
        view.Show();
        Instance.currentView = view;
    }
}

