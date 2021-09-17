using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestVREngine.Tunnel;
using TestVREngine.Util;

namespace TestVREngine.Scene
{
    abstract class GeneralScene
    {
        protected TunnelHandler Handler;

        protected GeneralScene(TunnelHandler handler)
        {
            this.Handler = handler;
        }

        public abstract void InitScene();

        public abstract void LoadScene();

        public virtual void SaveScene(string fileName)
        {
            this.Handler.SendToTunnel(JSONCommandHelper.WrapSaveScene(fileName), OnSaveCallback);
        }

        private void OnSaveCallback(string message)
        {
            Console.WriteLine("Scene save command returned from server");
        }
    }

}
