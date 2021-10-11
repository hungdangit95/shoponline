using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using ShopOnlineApp.Application.AutoMapper;

namespace ShopOnlineApp.Application.Common
{
    public abstract class ViewModelBase<TM, TMv> where TM : class where TMv : class
    {
        #region Public Medthods
        public TMv Map(TM item)
        {
            try
            {
                if (item == null) return null;
                return AutoMapperConfig.Config.CreateMapper().Map<TM, TMv>(item);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
        }

        public TM Map(TMv item)
        {
            if (item == null) return null;
            return AutoMapperConfig.Config.CreateMapper().Map<TMv, TM>(item);
        }

        public IEnumerable<TMv> Map(IEnumerable<TM> items)
        {
            try
            {
                if (items == null) return new List<TMv>();
                return AutoMapperConfig.Config.CreateMapper().Map<IEnumerable<TM>, IEnumerable<TMv>>(items);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
          
        }

        public IEnumerable<TM> Map(IEnumerable<TMv> items)
        {
            if (items == null) return new List<TM>();
            return AutoMapperConfig.Config.CreateMapper().Map<IEnumerable<TMv>, IEnumerable<TM>>(items);
        }

        #endregion Public Medthods

        #region Virtual Methods

        public virtual bool IsValid()
        {
            return true;
        }

        #endregion Abstract Methods
    }

}
