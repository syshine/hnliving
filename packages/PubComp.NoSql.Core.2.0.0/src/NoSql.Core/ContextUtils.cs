using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PubComp.NoSql.Core
{
    public static class ContextUtils
    {
        public static void LoadNavigation<TKey, TEntity>(
            IDomainContext domainContext, IEnumerable<TEntity> entities, IEnumerable<string> propertyNames)
            where TEntity : class, IEntity<TKey>
        {
            var entitiesByType = entities.GroupBy(e => e.GetType()).ToList();
            foreach (var entitiesOfType in entitiesByType)
            {
                var entityType = entitiesOfType.Key;
                var props = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => propertyNames.Contains(p.Name)).ToList();

                foreach (var navProp in props)
                {
                    if (!navProp.CanRead || !navProp.CanWrite || navProp.GetIndexParameters().Any())
                        continue;

                    var navAttr = navProp.GetCustomAttribute(typeof(NavigationAttribute)) as NavigationAttribute;
                    if (navAttr == null || string.IsNullOrEmpty(navAttr.IdPropertyName) || navAttr.EntitySetItemType == null)
                        continue;

                    var idProp = entityType.GetProperty(navAttr.IdPropertyName);
                    if (!idProp.CanRead || !idProp.CanWrite || idProp.GetIndexParameters().Any())
                        continue;

                    var isCollection = (navProp.PropertyType.GetInterface("IEnumerable") != null);

                    var set = domainContext.EntitySets.Where(s => s.EntityType == navAttr.EntitySetItemType).FirstOrDefault();
                    if (set == null)
                        continue;

                    if (!isCollection)
                    {
                        var idType = idProp.PropertyType;
                        var defaultId = idType.IsValueType ? Activator.CreateInstance(idType) : null;

                        foreach (var entity in entitiesOfType)
                        {
                            var id = idProp.GetValue(entity, null);
                            if (id == defaultId)
                                continue;

                            var obj = set.Get(id);
                            navProp.SetValue(entity, obj, null);
                        }
                    }
                    else
                    {
                        foreach (var entity in entitiesOfType)
                        {
                            var id = idProp.GetValue(entity, null);
                            var ids = id as IEnumerable;
                            var objs = set.Get(ids);
                            navProp.SetValue(entity, objs, null);
                        }
                    }
                }
            }
        }

        public static void SaveNavigation<TKey, TEntity>(
                IDomainContext domainContext, IEntitySet<TKey, TEntity> entitySet,
                IEnumerable<TEntity> entities, IEnumerable<string> propertyNames)
            where TEntity : class, IEntity<TKey>
        {
            var entitiesByType = entities.GroupBy(e => e.GetType()).ToList();
            foreach (var entitiesOfType in entitiesByType)
            {
                var entityType = entitiesOfType.Key;
                var props = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => propertyNames.Contains(p.Name)).ToList();

                foreach (var navProp in props)
                {
                    if (!navProp.CanRead || !navProp.CanWrite || navProp.GetIndexParameters().Any())
                        continue;

                    var navAttr = navProp.GetCustomAttribute(typeof(NavigationAttribute)) as NavigationAttribute;
                    if (navAttr == null || string.IsNullOrEmpty(navAttr.IdPropertyName) || navAttr.EntitySetItemType == null)
                        continue;

                    var idProp = entityType.GetProperty(navAttr.IdPropertyName);
                    if (!idProp.CanRead || !idProp.CanWrite || idProp.GetIndexParameters().Any())
                        continue;

                    var isCollection = (navProp.PropertyType.GetInterface("IEnumerable") != null);

                    var idType = idProp.PropertyType;
                    if (isCollection)
                    {
                        var idTypeArgs = idType.GetGenericArguments();
                        if (idTypeArgs.Count() != 1)
                            continue;

                        idType = idTypeArgs.First();
                    }

                    var defaultId = idType.IsValueType ? Activator.CreateInstance(idType) : null;

                    var set = domainContext.EntitySets.Where(s => s.EntityType == navAttr.EntitySetItemType).FirstOrDefault();
                    if (set == null)
                        continue;

                    var subEntityType = navProp.PropertyType;
                    if (isCollection)
                    {
                        var subEntityTypeArgs = subEntityType.GetGenericArguments();
                        if (subEntityTypeArgs.Count() != 1)
                            continue;

                        subEntityType = subEntityTypeArgs.First();
                    }
                    var subEntityIdProp = subEntityType.GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);

                    if (!isCollection)
                    {
                        foreach (var entity in entitiesOfType)
                        {
                            var obj = navProp.GetValue(entity, null) as IEntity;

                            if (obj == null)
                            {
                                idProp.SetValue(entity, defaultId, null);
                                continue;
                            }

                            var id = subEntityIdProp.GetValue(obj, null);

                            set.AddOrUpdate(obj);

                            idProp.SetValue(entity, id, null);
                        }
                    }
                    else
                    {
                        foreach (var entity in entitiesOfType)
                        {
                            var obj = navProp.GetValue(entity, null);
                            var objs = obj as IEnumerable<object>;
                            var ids = typeof(List<>).MakeGenericType(idType).GetConstructor(System.Type.EmptyTypes).Invoke(new object[0]) as IList;

                            if (objs != null)
                            {
                                foreach (IEntity o in objs)
                                {
                                    var id = subEntityIdProp.GetValue(o, null);

                                    set.AddOrUpdate(o);

                                    ids.Add(id);
                                }
                            }

                            idProp.SetValue(entity, ids, null);
                        }
                    }
                }
            }

            entitySet.Update(entities);
        }

        public static Type[] FindInheritingTypes(Assembly assembly, IEnumerable<Type> baseTypes)
        {
            var suitableTypes = new List<Type>();
            IEnumerable<Type> typesToExamine = assembly.GetTypes();

            suitableTypes.AddRange(typesToExamine.Where(t => baseTypes.Any(bt => t.IsSubclassOf(bt))));

            return suitableTypes.ToArray();
        }
    }
}
