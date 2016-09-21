using System.Reflection;

namespace FileManager.Web.References
{
    public static class ReferencedAssemblies
    {
        public static Assembly Services
        {
            get { return Assembly.Load("FileManager.Services"); }
        }

        public static Assembly Repositories
        {
            get { return Assembly.Load("FileManager.Data"); }
        }

        public static Assembly Dto
        {
            get
            {
                return Assembly.Load("FileManager.Dto");
            }
        }

        public static Assembly Domain
        {
            get
            {
                return Assembly.Load("FileManager.Core");
            }
        }
    }
}
