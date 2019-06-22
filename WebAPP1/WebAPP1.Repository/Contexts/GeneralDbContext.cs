using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPP1.Repository
{
    public class GeneralDbContext : DbContext
    {
        public GeneralDbContext(DbContextOptions options) : base(options)
        {

        }
        //public DbSet<Base_User> Base_Users { get; set; }
        //public DbSet<Base_UserToken> Base_Usertokens { get; set; }

        //public DbSet<Base_OperateLog> Base_OperateLogs { get; set; }

        //public DbSet<Base_RolePermission> Base_RolePermissions { get; set; }
        //public DbSet<Base_SysRole> Base_SysRoles { get; set; }
        //public DbSet<Base_UserRoleMap> Base_UserRoleMaps { get; set; }
        //public DbSet<Base_Menu> Base_Menus { get; set; }


    }
}
