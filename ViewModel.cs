using System.Collections.ObjectModel;
using System.Windows.Input;

namespace maui0; 

public class ViewModel {
    public ObservableCollection<Course> Items { get; } 
    public ICommand ItemTappedCommand { get; private set; } 

    public ViewModel() {
        Items = new ObservableCollection<Course>();
        ItemTappedCommand = new Command<Course>(OnItemTapped); 
    }

    private void OnItemTapped(Course Item) {
        Console.WriteLine($"You tap {Item.Name}. {Item.Instructor}, {Item.Schedule}");
    }

    public void LoadContent() {
        Items.Add(new Course { Name = "Course 1", Instructor = "Description 1" });
        Items.Add(new Course { Name = "Course 2", Instructor = "Description None" });
        for (int i = 0; i < 10; i++) {
            Items.Add(new Course { Name = $"C{i}", Instructor = $"DP {i}" });
        }
        return ; 
    }
}