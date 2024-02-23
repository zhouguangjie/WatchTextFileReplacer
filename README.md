# WatchTextFileReplacer
监控文件夹下的文本文件并替换文本

配置文件位置：%LocalAppData%\WatchTextFileReplacer\config.json

配置文件格式参考：WatchTextFileReplacer\config_template.json

## 运行模式
### Daemon
直接运行默认就是守护进程模式，实时监控文本文件并按照配置替换文本

### 手动执行执行一次文本替换任务
加-run参数运行，对配置里的所有任务做一次文本替换