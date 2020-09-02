using System.Collections.Generic;
using System.Threading.Tasks;

namespace DxSample.Shared.Repository {
    public interface IBaseRepostory<TDto> where TDto: class {
        Task<List<TDto>> Get();
        Task<TDto> Get(int id);
        Task<TDto> Create(TDto data);
        Task<TDto> Update(TDto data);
        Task Delete(int id);
    }
}
