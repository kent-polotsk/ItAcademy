﻿using Azure.Core;
using DAL_CQS_.Queries;
using EFDatabase;
using EFDatabase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_CQS_.QueryHandlers
{
    internal class TryLoginQueryHandler : IRequestHandler<TryLoginQuery, User?>
    {
        private readonly GNAggregatorContext _dbContext;

        public TryLoginQueryHandler(GNAggregatorContext dbContext) 
        { 
            _dbContext = dbContext;
        }
        public async Task<User?> Handle(TryLoginQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var foundUser = await _dbContext.Users
                    .AsNoTracking()
                    .Include(x => x.Role)
                    .SingleOrDefaultAsync(u => u.Email.Equals(request.Email), cancellationToken);

                return foundUser != null && request.PasswordHash.Equals(foundUser.PasswordHash)
                    ? foundUser
                    : null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
