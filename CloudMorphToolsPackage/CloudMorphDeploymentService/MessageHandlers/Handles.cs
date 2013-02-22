using CloudMorphDeploymentService.Messages;

namespace CloudMorphDeploymentService.MessageHandlers
{
    public interface Handles<T> where T : Message
    {
        void Handle(T message);
    }
}