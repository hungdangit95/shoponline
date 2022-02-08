using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShopOnlineApp.Helper
{
    public class ReflectionController
    {
        public List<Type> GetControllers(string namespaces)
        {
            List<Type> listController = new List<Type>();
            Assembly assembly = Assembly.GetExecutingAssembly();
            IEnumerable<Type> types =
                assembly.GetTypes()
                    .Where(type => typeof(Controller).IsAssignableFrom(type) && type.Namespace.Contains(namespaces))
                    .OrderBy(x => x.Name);
            return types.ToList();
        }

        //lấy danh sách các action theo controller truyền vào

        public List<string> GetActions(Type controller)
        {
            List<string> listAction = new List<string>();
            IEnumerable<MemberInfo> memberInfos =
                controller.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)
                    .Where(
                        m =>
                            !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute),
                                true).Any()).OrderBy(x => x.Name);
            foreach (MemberInfo method in memberInfos)
            {
                if (method.ReflectedType != null && (method.ReflectedType.IsPublic && !method.IsDefined(typeof(NonActionAttribute))))
                {
                    if (!listAction.Contains(method.Name))
                    {
                        listAction.Add(method.Name);
                    }
                }
            }
            return listAction;
        }
    }
}
