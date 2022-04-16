public struct StateTransision {
    public IMonoState From { get; private set; }
    public IMonoState To { get; private set; }

    public StateTransision(IMonoState from = null, IMonoState to = null) {
        this.From = from;
        this.To = to;
    }
}
