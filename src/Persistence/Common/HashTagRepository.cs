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
    public class HashTagRepository : GenericRepository<HashTag>, IHashTagRepository
    {
        private readonly IMapper _mapper;

        public HashTagRepository(IMapper mapper, ITwitterDbContext context) : base(context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<HashTagVm>> GetTopTags(string country, CancellationToken token) =>
            await _context.HashTags
                .Where(f => f.Country == country)
                .ProjectTo<HashTagVm>(_mapper.ConfigurationProvider)
                .ToListAsync(token);
    }
}
