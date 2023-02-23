using System.Collections.Generic;
using UnityEngine;
public class ViewManager : MonoBehaviour
{
    private List<View> views;
    private View startingView;
    private View currentView;    
    public int CurrentViewIndex { get; set; }
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
    public void Init(View startingView, List<View> views)
    {
        this.startingView = startingView;
        this.views = views;        
    }
    public void Show(int index)
    {
        if (index >= views.Count)
            return;

        var viewToDisplay = views[index];
        currentView?.Hide();
        viewToDisplay.Show();
        currentView = viewToDisplay;
       
    }
    public void Show(View view)
    {
        currentView?.Hide();
        view.Show();
        currentView = view;
    }
}

