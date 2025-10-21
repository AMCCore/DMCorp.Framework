using DMCorp.Framework.Basics.Settings;

namespace DMCorp.Framework.Basics.Email;

public interface IEmailService
{
    EmailServiceSettings Settings { get; }

    Task SendAsync(EmailSendDto model, CancellationToken token = default);

    void Send(EmailSendDto model);
}