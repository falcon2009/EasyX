using AutoMapper;
using EasyX.Data.Api.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace EasyX.Data.Core.Tool
{
    public static class MappingHelper
    {
        public static TEntity JoinOneToMany<TEntity, TModel, TFieldEntity, TFieldModel, TKey>(
            TEntity entity,
            TModel model,
            Expression<Func<TEntity, IList<TFieldEntity>>> fromEntity,
            Expression<Func<TModel, IList<TFieldModel>>> fromModel,
            IMapperBase mapper,
            bool updateEntities,
            bool isSoftDelete = true) where TFieldEntity : IKey<TKey> where TFieldModel : IKey<TKey> where TKey : class, IEquatable<TKey>
        {
            if (fromModel == null || fromEntity == null)
            {
                return entity;
            }
            //functions to get lists to map
            Func<TEntity, IList<TFieldEntity>> listEntityFunc = fromEntity.Compile();
            Func<TModel, IList<TFieldModel>> listModelFunc = fromModel.Compile();
            //get lists of source and destination
            IList<TFieldEntity> listFromEntity = listEntityFunc(entity);
            IList<TFieldModel> listFromModel = listModelFunc(model);
            //do one to many join
            listFromEntity = JoinOneToManyProperty<TFieldModel, TFieldEntity, TKey>(listFromModel, listFromEntity, mapper, updateEntities, isSoftDelete);
            //set new list to destination entity
            if (fromEntity.Body is MemberExpression entityDestinationMemberExpression && entityDestinationMemberExpression.Member is PropertyInfo entityDestinationPropertyInfo)
            {
                entityDestinationPropertyInfo.SetValue(entity, listFromEntity, null);
            }

            return entity;
        }

        public static IList<TFieldEntity> JoinOneToManyProperty<TFieldModel, TFieldEntity, TKey>(
            IList<TFieldModel> sourceList, 
            IList<TFieldEntity> destinationList, 
            IMapperBase mapper, 
            bool updateEntities,
            bool isSoftDelete) where TFieldEntity : IKey<TKey> where TFieldModel : IKey<TKey> where TKey : class, IEquatable<TKey>
        {
            destinationList ??= new List<TFieldEntity>();
            
            sourceList ??= new List<TFieldModel>();

            List<TFieldEntity> entityToRemoveList= new List<TFieldEntity>();
            //check items that have been removed
            foreach (TFieldEntity entity in destinationList)
            {
                TFieldModel model = sourceList.FirstOrDefault(item => item.Key?.Equals(entity.Key)??false);
                if (model == null)
                {
                    if (isSoftDelete && entity is IDeleteTrackable entityDeletable)
                    {
                        entityDeletable.DeletedOn = DateTimeOffset.Now;
                    }
                    else
                    {
                        entityToRemoveList.Add(entity);
                    }
                }
            }
            //remove deleted items from destination list
            foreach(TFieldEntity entity in entityToRemoveList)
            {
                destinationList.Remove(entity);
            }
            //add new items to destination list or update existing if requered
            foreach (TFieldModel model in sourceList)
            {
                if (mapper == null)
                {
                    throw new ArgumentNullException(nameof(mapper), "Mapper cannot be null");
                }

                TFieldEntity entity = destinationList.FirstOrDefault(item => item.Key.Equals(model.Key));
                if (entity == null || entity.Key == null || entity.Key == default)
                {
                    entity = mapper.Map<TFieldEntity>(model);
                    destinationList.Add(entity);
                }
                else
                {
                    if (updateEntities)
                    {
                        mapper.Map(model, entity);
                    }
                }
            }
            return destinationList;
        }
    }
}
