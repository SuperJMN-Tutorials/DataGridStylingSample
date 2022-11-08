using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using ReactiveUI;

namespace DataGridStylingSample.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        var children = new ObservableCollection<TreeNode>();
        var rootNode = new TreeNode(new ViewModel("Root"), children);

        Observable
            .Interval(TimeSpan.FromSeconds(1), RxApp.MainThreadScheduler)
            .Subscribe(l => children.Add(new TreeNode(new ViewModel($"Child {l}"))));

        var items = new ObservableCollection<TreeNode>(new ObservableCollection<TreeNode> {rootNode});

        var nameColumn = new TextColumn<TreeNode, string>("Name", treeNode => ((ViewModel) treeNode.Value).Name);

        Source1 = new HierarchicalTreeDataGridSource<TreeNode>(items)
        {
            Columns =
            {
                new HierarchicalExpanderColumn<TreeNode>(nameColumn, x => x.Children, x => true),
            }
        };

        Source2 = new HierarchicalTreeDataGridSource<TreeNode>(items)
        {
            Columns =
            {
                new HierarchicalExpanderColumn<TreeNode>(nameColumn, x => x.Children, x => x.Children.Any()),
            }
        };
    }

    public HierarchicalTreeDataGridSource<TreeNode> Source2 { get; }

    public HierarchicalTreeDataGridSource<TreeNode> Source1 { get; }
}

public class ViewModel
{
    public ViewModel(string name)
    {
        Name = name;
    }

    public string Name { get; }
}

public class TreeNode
{
    public TreeNode(object value) : this(value, Enumerable.Empty<TreeNode>())
    {
    }

    public TreeNode(object value, IEnumerable<TreeNode> children)
    {
        Value = value;
        Children = children;
    }

    public object Value { get; }
    public IEnumerable<TreeNode> Children { get; }

    public override string? ToString()
    {
        return Value.ToString();
    }
}