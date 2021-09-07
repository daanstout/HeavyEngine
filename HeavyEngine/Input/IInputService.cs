namespace HeavyEngine.Input {
    /// <summary>
    /// A service that handles input
    /// </summary>
    public interface IInputService {
        /// <summary>
        /// Creates a <see cref="TriggerKey"/>
        /// <para><see cref="TriggerKey"/>s are keys that get triggered when their key-combination is active and an event that can be subscribed to.</para>
        /// </summary>
        /// <param name="context">The context that requests the key to be made (for automatically cleaning up loose keys)</param>
        /// <param name="keyName">The name of the <see cref="TriggerKey"/> (optional)</param>
        /// <param name="keyInput">The input type this <see cref="TriggerKey"/> should listen to</param>
        /// <returns>A newly created <see cref="TriggerKey"/> that can be set up</returns>
        TriggerKey CreateTriggerKey(object context, string keyName = "", KeyInput keyInput = KeyInput.Pressed);
        /// <summary>
        /// Creates a <see cref="VelocityKey"/>
        /// <para><see cref="VelocityKey"/>s are keys that allow you to obtain a value from -1 to 1, based on subscribed keys being pressed.</para>
        /// </summary>
        /// <param name="context">The context that requests the key to be made (for automatically cleaning up loose keys)</param>
        /// <returns></returns>
        VelocityKey CreateVelocityKey(object context);
        /// <summary>
        /// Destroys a <see cref="TriggerKey"/> so that it isn't used anymore
        /// </summary>
        /// <param name="actionKey">The <see cref="TriggerKey"/> to remove</param>
        /// <returns><see langword="true"/> if the <see cref="TriggerKey"/> was succesfully removed</returns>
        bool DestroyKey(TriggerKey actionKey);
        /// <summary>
        /// Destroys a <see cref="VelocityKey"/> so that it isn't used anymore
        /// </summary>
        /// <param name="key">The <see cref="VelocityKey"/> to remove</param>
        /// <returns><see langword="true"/> if the <see cref="VelocityKey"/> was succesfully removed</returns>
        bool DestroyKey(VelocityKey key);
    }
}