using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using FluentValidation.Results;

namespace ArlequimTech.Core.BaseClasses;

[ExcludeFromCodeCoverage]
public abstract class NotifiableEntity
{
    private readonly List<Notification> _notifications;

    protected NotifiableEntity() { _notifications = new List<Notification>(); }

    public void AddNotification(string property, string message)
    {
        _notifications.Add(new Notification(property, message));
    }

    public void AddNotifications(IEnumerable<Notification> validations)
    {
        _notifications.AddRange(validations);
    }

    public void AddNotifications(IEnumerable<ValidationFailure> validations)
    {
        foreach(var failure in validations)
        {
            _notifications.Add(new Notification(failure.PropertyName, failure.ErrorMessage));
        }
    }

    public void AddNotifications(params NotifiableEntity[] items)
    {
        foreach (var item in items)
            AddNotifications(item);
    }

    protected virtual IEnumerable<Notification> Validations() => null;

    public IEnumerable<Notification> Notifications()
    {
        return _notifications ?? new List<Notification>();
    }
    [JsonIgnore]
    public bool IsValid => !_notifications.Any();
}