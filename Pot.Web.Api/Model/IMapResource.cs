namespace Pot.Web.Api.Model
{
    public interface IMapResource<T, out TResource>
    {
        TResource MapFrom(T entity);

        T MapTo();

        T MapTo(T entity);
    }
}