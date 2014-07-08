using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace VirtualPathProvider
{
    /// <summary>
    /// MasterPage Virtual File
    /// </summary>
    public class MasterPageVirtualFile : VirtualFile
    {
        private string virPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterPageVirtualFile"/> class.
        /// </summary>
        /// <param name="virtualPath">The virtual path to the resource represented by this instance.</param>
        public MasterPageVirtualFile(string virtualPath)
            : base(virtualPath)
        {
            this.virPath = virtualPath;
        }

        /// <summary>
        /// When overridden in a derived class, returns a read-only stream to the virtual resource.
        /// </summary>
        /// <returns>A read-only stream to the virtual file.</returns>
        public override Stream Open()
        {
            if (!(HttpContext.Current == null))
            {
                if (HttpContext.Current.Cache[virPath] == null)
                {
                    HttpContext.Current.Cache.Insert(virPath, ReadResource(virPath));
                }
                return (Stream)HttpContext.Current.Cache[virPath];
            }
            else
            {
                return ReadResource(virPath);
            }
        }

        private static Stream ReadResource(string embeddedFileName)
        {
            string resourceFileName = VirtualPathUtility.GetFileName(embeddedFileName);
            Assembly assembly = Assembly.GetExecutingAssembly();
            return assembly.GetManifestResourceStream(MasterPageVirtualPathProvider.VirtualPathProviderResourceLocation + "." + resourceFileName);
        }
    }
}
