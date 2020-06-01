namespace Hellhole.FastContainer
{
    public enum LifetimeScope
    {
        /// <summary>
        /// Each injection receives its own instance.
        /// </summary>
        InstancePerRequest,
        /// <summary>
        /// Each injection receives the same app-global instance.
        /// </summary>
        Singleton
    }
}
