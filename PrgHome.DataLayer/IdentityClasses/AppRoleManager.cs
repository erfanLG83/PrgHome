using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Logging;
using PrgHome.DataLayer.IdentityServices;

namespace PrgHome.DataLayer.IdentityClasses
{
    public class AppRoleManager : RoleManager<AppRole>, IAppRoleManager
    {

        private readonly IRoleStore<AppRole> _store;
        private readonly IEnumerable<IRoleValidator<AppRole>> _roleValidators;
        private readonly ILookupNormalizer _keyNormalizer;
        private readonly IdentityErrorDescriber _errors;
        private readonly ILogger<RoleManager<AppRole>> _logger;
        public AppRoleManager(
            IRoleStore<AppRole> roleStore,
            IEnumerable<IRoleValidator<AppRole>> roleValidators,
            ILookupNormalizer lookupNormalizer,
            IdentityErrorDescriber errorDescriber,
            ILogger<RoleManager<AppRole>> logger
            )
            : base(roleStore, roleValidators, lookupNormalizer, errorDescriber, logger)
        {
            _store = roleStore;
            _roleValidators = roleValidators;
            _keyNormalizer = lookupNormalizer;
            _errors = errorDescriber;
            _logger = logger;
        }

        public async Task<AppRole> FindByIdWithUserRolesAsync(string id)
        {
            return await Roles.Include(n => n.UserRoles).FirstAsync(n => n.Id == id);

        }

        public async Task<IEnumerable<AppRole>> GetRoles()
        {
            return await Roles.Include(n => n.UserRoles).ToListAsync();
        }
        
    }
}
