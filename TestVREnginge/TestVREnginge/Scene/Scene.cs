using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestVREngine.Tunnel;

namespace TestVREngine.Scene
{
    abstract class Scene
    {
        private readonly TunnelHandler Handler;

        public abstract void InitScene(TunnelHandler handler);

        public abstract void LoadScene();

        public virtual void SaveScene()
        {

        }
    }

}
