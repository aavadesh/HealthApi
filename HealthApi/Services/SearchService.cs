using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using AutoMapper;
using HealthApi.Entities;
using HealthApi.Exceptions;
using HealthApi.Extensions;
using HealthApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthApi.Services
{
    public interface ISearchService
    {
        List<SearchDto> GetByString(string searchText);
    }

    public class SearchService : ISearchService
    {
        private readonly HealthDbContext _context;
        private readonly IMapper _mapper;

        public SearchService(HealthDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public List<SearchDto> GetByString(string searchText)
        {
            //var bookContent = String.Join(",", _context.BookContents.Select(p => p.PageNumber));
            string sql = @"select t2.Name as BookName,t2.Id as BookId, t4.Name, t4.Surname,
                                   isnull(STUFF(
                                       (SELECT distinct ', ' + CONVERT(varchar(10),t1.PageNumber)
	                                  FROM BookContents t1 inner join Books t on t1.BookId = t.Id 
	                                  where t1.Content like '%" + searchText + "%' and t2.Id = t.Id FOR XML PATH('')), 1, 1, ''), 'Search Phase not found') Page "+
                                      "from Books t2 inner join AuthorBooks t3 on t2.Id = t3.BookId "+
	                                  "inner join Authors t4 on t3.AuthorId = t4.Id "+
	                                  "Where t2.Name like '%" + searchText + "%' or t4.Name like '%" + searchText + "%' or t4.Surname like '%" + searchText + "%' ";
            var objctx = (_context as IObjectContextAdapter).ObjectContext;

            ObjectQuery<SearchDto> searchResult = objctx.CreateQuery<SearchDto>(sql);



            List<SearchDto> result = searchResult.ToList();

            if (result is null)
            {
                throw new NotFoundException("Book not found");
            }

            return result;
        }
    }
}
