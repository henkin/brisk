using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TemplateLibrary
{
    //public class AppViewCollection : AppViewPAppViewModelroperty
    //{
    //    public IEnumerable<AppViewModel> Items { get; set; }
    //}
    public class AppViewListModel : AppViewModel {}

    public class AppViewModel
    {
        public string Type { get; set; }
        public string VariableName { get; set; }
        public List<AppViewModel> Items { get; set; }
        public bool SimpleType { get; set; }

        public string Value { get; set; }

        public AppViewModel()
        {
            Items = new List<AppViewModel>();
            SimpleType = false;
        }

        

        public static AppViewModel Create(object o, string variableName = null)
        {
            var type = o.GetType();
            var appViewModel = new AppViewModel();
            appViewModel.Type = type.Name;
            appViewModel.VariableName = variableName;

            foreach (var prop in type.GetProperties())
            {
                if (prop.PropertyType.Name == ("List`1")) // heh
                {
                    appViewModel.Items.Add(new AppViewListModel()
                            {
                                Items = (prop.GetValue(o, null) as IList)
                                    .Cast<object>()
                                    .Select(i => Create(i))
                                    .ToList(),
                                Type = prop.PropertyType.GetGenericArguments().First().Name,
                                VariableName = prop.Name
                            });
                }
                else if ((prop.PropertyType.IsClass || prop.PropertyType.IsInterface) && !(prop.PropertyType == typeof(string)))
                {
                    appViewModel.Items.Add(Create(prop.GetValue(o, null), prop.Name));
                }
                else
                {
                    appViewModel.Items.Add(
                        new AppViewModel()
                        {
                            Type = prop.PropertyType.Name,
                            VariableName = prop.Name,
                            Value = prop.GetValue(o, null).ToString(),
                            SimpleType = true
                        });
                }
            }

            return appViewModel;
        }

        public IEnumerable<string> GetTypeNames()
        {
            return GetAppViewModelsTypes().Select(p => p.Type);
        }

        public IEnumerable<AppViewModel> GetAppViewModelsTypes()
        {
            var listOfItems = Items
                .Flatten(p => p.Items)
                .Where(p => !p.SimpleType)
                .Distinct()
                .ToList();

            listOfItems.Insert(0, this);
            return listOfItems;
        }
    }
    
    public static class Ext
    {
        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> source,
                                             Func<T, IEnumerable<T>> childrenSelector)
        {
            // Do standard error checking here.
            //if (source == null)
            

            // Cycle through all of the items.
            foreach (T item in source)
            {
                // Yield the item.
                yield return item;

                // Yield all of the children.
                foreach (T child in Flatten(childrenSelector(item), childrenSelector))
                {
                    // Yield the item.
                    yield return child;
                }
            }
        }
    }

    
}