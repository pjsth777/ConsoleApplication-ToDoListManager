using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

class Program
{
    static List<Task> tasks = new List<Task>();
    static int taskIdCounter = 1;

    static void Main(string[] args)
    {
        LoadTasksFromFile(); // Load tasks on startup

        bool running = true;

        while (running)
        {
            Console.WriteLine("\nTo-Do List Manager");
            Console.WriteLine("1. Add a task");
            Console.WriteLine("2. View tasks");
            Console.WriteLine("3. Edit a task");
            Console.WriteLine("4. Delete a task");
            Console.WriteLine("5. Save and Exit");
            Console.WriteLine("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddTask();
                    break;
                case "2":
                    ViewTasks();
                    break;
                case "3":
                    EditTask();
                    break;
                case "4":
                    DeleteTask();
                    break;
                case "5":
                    SaveTasksToFile();
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    public class Task
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }

        public Task(int id, string description)
        {
            Id = id;
            Description = description;
            IsCompleted = false; // By default, tasks are not completed
        }

        public override string ToString()
        {
            return $"{Id}. {Description} - {(IsCompleted ? "Completed" : "Pending")}";
        }
    }

    static void AddTask()
    {
        Console.Write("Enter task description: ");
        string description = Console.ReadLine();

        Task newTask = new Task(taskIdCounter, description);
        tasks.Add(newTask);
        taskIdCounter++;

        Console.WriteLine("Task added successfully.");
    }

    static void ViewTasks()
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("No tasks to display.");
            return;
        }

        Console.WriteLine("\nTo-Do List:");
        foreach(Task task in tasks)
        {
            Console.WriteLine(task.ToString());
        }
    }


    static void EditTask()
    {
        Console.WriteLine("Enter the ID of the task to edit: ");
        if (int.TryParse(Console.ReadLine(), out int taskId))
        {
            Task taskToEdit = tasks.Find(t => t.Id == taskId);
            if (taskToEdit != null)
            {
                Console.WriteLine($"Editing task: {taskToEdit}");
                Console.WriteLine("1. Update Description");
                Console.WriteLine("2. Mark as Completed");
                string choice = Console.ReadLine();

                switch(choice)
                {
                    case "1":
                        Console.Write("Enter new description: ");
                        taskToEdit.Description = Console.ReadLine();
                        Console.WriteLine("Task description updated.");
                        break;
                    case "2":
                        taskToEdit.IsCompleted = true;
                        Console.WriteLine("Task marked as completed.");
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

            }
            else
            {
                Console.WriteLine("Task not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid task ID.");
        }
    }


    static void DeleteTask()
    {
        Console.WriteLine("Enter the ID of the task to delete: ");
        if (int.TryParse(Console.ReadLine(), out int taskId))
        {
            Task taskToDelete = tasks.Find(t => t.Id == taskId);
            if (taskToDelete != null)
            {
                tasks.Remove(taskToDelete);
                Console.WriteLine("Task Deleted.");
            }
            else
            {
                Console.WriteLine("Task not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid task ID.");
        }
    }

    static void SaveTasksToFile()
    {
        using (StreamWriter writer = new StreamWriter("tasks.txt"))
        {
            foreach(Task task in tasks)
            {
                writer.WriteLine($"{task.Id},{task.Description},{task.IsCompleted}");
            }
        }
        Console.WriteLine("Tasks saved to file.");
    }

    static void LoadTasksFromFile()
    {
        if (File.Exists("tasks.txt"))
        {
            using (StreamReader reader = new StreamReader("tasks.txt"))
            {
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(",");
                    int id = int.Parse(parts[0]);
                    string description = parts[1];
                    bool IsCompleted = bool.Parse(parts[2]);

                    Task task = new Task(id, description);
                    task.IsCompleted = IsCompleted;
                    tasks.Add(task);
                    taskIdCounter = Math.Max(taskIdCounter, id + 1); // Ensure taskIdConter is always higher than the highest ID
                }
            }
        }
    }

}



