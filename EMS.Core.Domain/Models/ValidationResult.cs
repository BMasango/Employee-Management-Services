namespace EMS.Core.Domain.Models
{
    /// <summary>
    /// ValidationResult
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// If any messages are passed through value will be false
        /// </summary>
        public bool IsValid => !this.ValidationMessages.Any();
        /// <summary>
        /// List of messages
        /// </summary>
        public List<string> ValidationMessages { get; set; } = new List<string>();
    }

    public class ValidationResult<T> : ValidationResult
    {
        /// <summary>
        /// Result or response object
        /// </summary>
        public T Data { get; set; }
    }
}
