using CommunityToolkit.Mvvm.Messaging.Messages;

namespace TaskStack.Messages;

public class TaskSelectedMessage(int id) : ValueChangedMessage<int>(id);