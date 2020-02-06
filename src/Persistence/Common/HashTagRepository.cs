using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Common
{
    public class HashTagRepository : BaseRepository<HashTag>, IHashTagRepository
    {
        private readonly IMapper _mapper;

        public HashTagRepository(IMapper mapper, ITwitterDbContext context) : base(context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<HashTag> GetTag(string tag, CancellationToken token) =>
            Query.FirstOrDefaultAsync(f => f.Tag == tag, token);

        public Task<List<HashTagVm>> GetTopTags(string country, CancellationToken token) =>
            Query.Where(f => f.Country == country)
                .ProjectTo<HashTagVm>(_mapper.ConfigurationProvider)
                .ToListAsync(token);

        public Task<List<HashTagVm>> SearchTags(string search, CancellationToken token) =>
            Query.Where(f => EF.Functions.Like(f.Tag, '%' + search + '%'))
                .ProjectTo<HashTagVm>(_mapper.ConfigurationProvider)
                .ToListAsync(token);
    }
}
