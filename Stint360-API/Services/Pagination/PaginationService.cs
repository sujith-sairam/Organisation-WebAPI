using AutoMapper;
using Organisation_WebAPI.InputModels;
using Organisation_WebAPI.ViewModels;

namespace Organisation_WebAPI.Services.Pagination
{
    public class PaginationService<T, R> : IPaginationServices<T, R> where T : class
    {
        private readonly IMapper _mapper;

        public PaginationService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public PaginationResultVM<T> GetPagination(List<R> source, PaginationInput paginationInput)
        {
            var currentPage = paginationInput.PageNumber;

            var totalNoOfRecords = source.Count;

            var pageSize = paginationInput.PageSize;

            var results = source.Skip((paginationInput.PageNumber - 1) * (paginationInput.PageSize))
                .Take(paginationInput.PageSize).ToList(); 

            var items = _mapper.Map<List<T>>(results);

            var totalPages = (int)Math.Ceiling(totalNoOfRecords / (double)pageSize);

            PaginationResultVM<T> paginationResultVM = new PaginationResultVM<T>(currentPage,
                totalNoOfRecords, pageSize, totalPages, items);

            return paginationResultVM;
        }
    }
}
