using System;

public interface ICollectable {
    public event Action OnCollect;
    void Collect();
}
