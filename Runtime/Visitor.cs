public interface IVisitor<in TVisitable> where TVisitable : IVisitable {
    void Visit(TVisitable visitable);
    void DefaultVisit(TVisitable visitable);
}

public interface IVisitable {
    public void Accept(IVisitor<IVisitable> visitor);
}