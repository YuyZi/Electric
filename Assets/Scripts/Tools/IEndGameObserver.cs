
public interface IEndGameObserver
{
    //结束游戏广播
    //接口中只写定义不写方法语言实现  ，而使用该接口的程序中才写入方法的内容
    void EndNotify();
}
