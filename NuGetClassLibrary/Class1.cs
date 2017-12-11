using System.ServiceModel;

namespace NuGetClassLibrary
{
    public class Class1
    {
        public Class1()
        {
            var class2 = new Class2();
            using (var client = new ChannelFactory<object>())
            {
                
            }
        }
    }
}
