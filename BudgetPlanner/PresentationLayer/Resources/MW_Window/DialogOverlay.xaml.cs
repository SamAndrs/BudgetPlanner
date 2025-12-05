using System.Windows;
using System.Windows.Controls;

namespace BudgetPlanner.PresentationLayer.Resources.MW_Window
{
   
    public partial class DialogOverlay : UserControl
    {
        public static readonly DependencyProperty IsOpenProperty =
       DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(DialogOverlay));

        public static readonly DependencyProperty DialogContentProperty =
            DependencyProperty.Register(nameof(DialogContent), typeof(object), typeof(DialogOverlay));

        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }

        public object DialogContent
        {
            get => GetValue(DialogContentProperty);
            set => SetValue(DialogContentProperty, value);
        }

        public DialogOverlay()
        {
            InitializeComponent();
        }
    }
}
