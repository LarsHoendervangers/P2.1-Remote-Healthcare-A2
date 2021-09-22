using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestVREngine.Tunnel;
using TestVREngine.Util;

namespace TestVREngine.Scene
{
    /// <summary>
    /// Abstract class the represents a Scene
    /// Scenes can:
    /// <ul>
    ///     <li>Be inititalized</li>
    ///     <li>Be loaded to the server</li>
    ///     <li>Be saved to the server</li>
    /// </ul>
    /// </summary>
    abstract class GeneralScene
    {
        protected TunnelHandler Handler;

        /// <summary>
        /// Constructor for GeneralScene
        /// </summary>
        /// <param name="handler">The TunnelHandler that the scene needs to communicate to the server with</param>
        protected GeneralScene(TunnelHandler handler)
        {
            this.Handler = handler;
        }

        /// <summary>
        /// Inits the scene, all action needed to start are preformed
        /// </summary>
        public abstract void InitScene();

        /// <summary>
        /// Loads the scene in the server
        /// </summary>
        public abstract void LoadScene();

        /// <summary>
        /// Save the current scene on the server, given the filename
        /// </summary>
        /// <param name="fileName">The filename to store the scene to</param>
        public virtual void SaveScene(string fileName)
        {
            this.Handler.SendToTunnel(JSONCommandHelper.WrapSaveScene(fileName), OnSaveCallback);
        }

        /// <summary>
        /// Callback fucntion called to handle the save response from the server
        /// </summary>
        /// <param name="message">The message received from the server</param>
        private void OnSaveCallback(string message)
        {
            //TODO handle error from server to user
            Trace.WriteLine($"Scene: save command returned from server: {message} \n");
            Console.WriteLine("Scene save command returned from server");
        }
    }

}
