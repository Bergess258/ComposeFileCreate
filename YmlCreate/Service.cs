using System;
using System.Collections.Generic;

namespace YmlCreate
{
    [Serializable]
    public class Service
    {
        public string Name { get; set; }
        public byte[] Img { get; set; }

        public Service(string n, byte[] i)
        {
            Name = n;
            Img = i;
        }
        public Service(string n)
        {
            Name = n;
        }
    }
    [Serializable]
    public class AllServices
    {
        public static List<Service> services = new List<Service>();
    }
    static class AllServiceOptions
    {
        //Definitions for options used more than once
        static readonly Options cpus = new Options("cpus", ValueType.One);
        static readonly Options memory = new Options("memory", ValueType.One);
        static readonly Options delay = new Options("delay", ValueType.Seconds);
        static readonly Options max_size = new Options("max-size", ValueType.One);
        static readonly Options max_file = new Options("max-file", ValueType.Number);
        static readonly Options compress = new Options("compress", ValueType.Bool);
        static readonly Options env = new Options("env", ValueType.One);
        static readonly Options env_regex = new Options("env-regex", ValueType.One);
        static readonly Options tag = new Options("tag", ValueType.One);
        static readonly Options external = new Options("external", ValueType.Bool);
        static readonly Options name = new Options("name", ValueType.One);
        static readonly Options file = new Options("file", ValueType.One);
        static readonly Options driver_opts = new Options("driver_opts", ValueType.ListWithValue);
        static readonly Options driver = new Options("driver", ValueType.One);//for networks options
        static readonly Options externalName = new Options("external", ValueType.Empty,new List<Options>() { new Options("Name",ValueType.One)});

        
        static readonly List<Options> ConfigNSecret = new List<Options>()
            {
                {new Options("source",ValueType.One)},
                {new Options("target",ValueType.One)},
                {new Options("uid",ValueType.Number)},
                {new Options("gid",ValueType.Number)},
                {new Options("mode",ValueType.One)},
            };
        static readonly List<Options> RollbackNUpdateConfig = new List<Options>()
                        {
                            new Options("parallelism",ValueType.Number),
                            delay,
                            new Options("failure_action",ValueType.One),
                            new Options("monitor",ValueType.Number),
                            new Options("max_failure_ratio",ValueType.Number),
                            new Options("order",ValueType.One)
                        };

        //Combined by configuration reference into different lists 
        static readonly List<Options> GlobalOptions = new List<Options>() {
            {new Options("args",ValueType.List) },
            {new Options("labels",ValueType.ListWithValue)},
            {new Options("build",ValueType.OneOrEmpty, new List<Options>()
                {
                    GlobalOptions[0],
                    {new Options("context",ValueType.One)},
                    {new Options("dockerfile",ValueType.One)},
                    {new Options("cache_from",ValueType.One)},
                    GlobalOptions[1],
                    {new Options("network",ValueType.One)},
                    {new Options("shm_size",ValueType.One)},
                    {new Options("target",ValueType.One)}
                }
            ) },
            {new Options("cap_add",ValueType.List)},
            {new Options("cap_drop",ValueType.List)},
            {new Options("cgroup_parent",ValueType.One)},
            {new Options("command",ValueType.One)},
            {new Options("configs",ValueType.List){ AdditionalInfo="Short"} },
            {new Options("configs",ValueType.List,ConfigNSecret){ AdditionalInfo="Long"} },
            {new Options("container_name",ValueType.One)},
            {new Options("container_name",ValueType.One)},
            {new Options("depends_on",ValueType.List)},
            {new Options("deploy",ValueType.Empty, new List<Options>()
                {
                    {new Options("endpoint_mode",ValueType.One)},
                    GlobalOptions[1],
                    {new Options("mode",ValueType.One)},
                    {new Options("placement",ValueType.Empty)},
                    {new Options("replicas",ValueType.Number)},
                    {new Options("max_replicas_per_node",ValueType.Number)},
                    {new Options("resources",ValueType.Empty, 
                        new List<Options>()
                        {
                            new Options("limits",ValueType.Empty, new List<Options>(){cpus,memory }),
                            new Options("reservations",ValueType.Empty, new List<Options>(){cpus,memory })
                        }
                        )},
                    {new Options("restart_policy",ValueType.Empty,
                        new List<Options>()
                        {
                            new Options("condition",ValueType.One),
                            delay,
                            new Options("max_attempts",ValueType.Number),
                            new Options("window",ValueType.Seconds)
                        }
                        )},
                    {new Options("rollback_config",ValueType.Empty,RollbackNUpdateConfig)},
                    {new Options("update_config",ValueType.Empty,RollbackNUpdateConfig)}
                }
            ) },
            {new Options("devices",ValueType.List)},
            {new Options("dns",ValueType.OneOrList)},
            {new Options("dns_search",ValueType.OneOrList)},
            {new Options("entrypoint",ValueType.One)},
            {new Options("env_file",ValueType.OneOrList)},
            {new Options("environment",ValueType.List)},
            {new Options("expose",ValueType.List)},
            {new Options("external_links",ValueType.List)},
            {new Options("extra_hosts",ValueType.List)},
            {new Options("healthcheck",ValueType.Empty, new List<Options>()
                {
                    {new Options("test",ValueType.Special)},
                    {new Options("interval",ValueType.Special)},
                    {new Options("timeout",ValueType.Seconds)},
                    {new Options("start_period",ValueType.Seconds)},
                    {new Options("retries",ValueType.Number)}
                }
            ) },
            {new Options("image",ValueType.One)},
            {new Options("isolation",ValueType.ComboBox)},
            {new Options("links",ValueType.List)},
            {new Options("logging",ValueType.Empty,new List<Options>()
            {
                {new Options("driver",ValueType.ComboBox)},
                {new Options("options",ValueType.Special,new List<Options>() //https://docs.docker.com/config/containers/logging/configure/
                    {
                        new Options("local",ValueType.Special,new List<Options>()
                        {
                            max_file,
                            max_size,
                            compress
                        }),
                        new Options("json-file",ValueType.Special,new List<Options>()
                        {
                            max_file,
                            max_size,
                            compress,
                            env,
                            env_regex
                        }),
                        new Options("journald",ValueType.Special,new List<Options>(){ tag,env,env_regex}),
                        new Options("syslog",ValueType.Special,new List<Options>(){
                            env,env_regex,tag,
                            {new Options("syslog-address",ValueType.One)},
                            {new Options("syslog-facility",ValueType.One)},
                            {new Options("syslog-tls-ca-cert",ValueType.One)},
                            {new Options("syslog-tls-cert",ValueType.One)},
                            {new Options("syslog-tls-key",ValueType.One)},
                            {new Options("syslog-format",ValueType.One)},
                            {new Options("syslog-tls-skip-verify",ValueType.Bool)}
                        }),
                        new Options("gelf",ValueType.Special,new List<Options>(){
                            tag,env,env_regex,
                            {new Options("gelf-address",ValueType.One)},
                            {new Options("gelf-compression-type",ValueType.ComboBox)}, //gzip,zlib,none https://docs.docker.com/config/containers/logging/gelf/
                            {new Options("gelf-compression-level",ValueType.Number)},// from -1 to 9 Default 1(BestSpeed)/ 0 and -1 disables compression
                            {new Options("gelf-tcp-max-reconnect",ValueType.Number)},// >0 default 3
                            {new Options("gelf-tcp-reconnect-delay",ValueType.Number)}// >0 default 1
                        }),
                        new Options("fluentd",ValueType.Special,new List<Options>(){
                            tag,env,env_regex,
                            {new Options("fluentd-address",ValueType.One)},
                            {new Options("fluentd-async-connect",ValueType.Bool)},
                            {new Options("fluentd-buffer-limit",ValueType.One)}, //10KB as example
                            {new Options("fluentd-retry-wait",ValueType.Seconds)},
                            {new Options("fluentd-max-retries",ValueType.Number)},
                            {new Options("fluentd-sub-second-precision",ValueType.Bool)}
                        }),
                        new Options("awslogs",ValueType.Special,new List<Options>(){
                            tag,env,env_regex,
                            {new Options("awslogs-region",ValueType.One)},
                            {new Options("awslogs-endpoint",ValueType.One)},
                            {new Options("awslogs-group",ValueType.One)},
                            {new Options("awslogs-create-group",ValueType.Bool)},
                            {new Options("awslogs-datetime-format",ValueType.One)},
                            {new Options("awslogs-multiline-pattern",ValueType.One)}
                        }),
                        new Options("splunk",ValueType.Special,new List<Options>(){
                            tag,env,env_regex,
                            {new Options("splunk-token",ValueType.One)},
                            {new Options("splunk-url",ValueType.One)},
                            {new Options("splunk-source",ValueType.One)},
                            {new Options("splunk-sourcetype",ValueType.One)},
                            {new Options("splunk-index",ValueType.One)},
                            {new Options("splunk-capath",ValueType.One)},
                            {new Options("splunk-insecureskipverify",ValueType.Bool)},
                            {new Options("splunk-format",ValueType.ComboBox)},
                            {new Options("splunk-verify-connection",ValueType.Bool)},
                            {new Options("splunk-gzip",ValueType.Bool)},
                            {new Options("splunk-gzip-level	",ValueType.Number)}
                        }),
                        new Options("etwlogs",ValueType.Special,new List<Options>(){
                            {new Options("container_name",ValueType.One)},
                            {new Options("image_name",ValueType.One)},
                            {new Options("container_id",ValueType.One)},
                            {new Options("image_id",ValueType.One)},
                            {new Options("source",ValueType.ComboBox)},
                            {new Options("log",ValueType.One)}
                        }),
                        new Options("gcplogs",ValueType.Special,new List<Options>(){
                            env,env_regex,
                            {new Options("gcp-project",ValueType.One)},
                            {new Options("gcp-log-cmd",ValueType.Bool)},
                            {new Options("gcp-meta-zone",ValueType.One)},
                            {new Options("gcp-meta-name",ValueType.One)},
                            {new Options("gcp-meta-id",ValueType.ComboBox)}
                        }),
                        new Options("logentries",ValueType.Special,new List<Options>(){
                            env,env_regex,
                            {new Options("logentries-token",ValueType.One)},
                            {new Options("line-only",ValueType.Bool)}
                        }),
                    }
                )},
            })},
            {new Options("network_mode",ValueType.One)},
            {new Options("networks",ValueType.List)}, //Should ckeck all current networks //**info for me**
            {new Options("networks",ValueType.Special)}, //For aliases and IPV4_ADDRESS, IPV6_ADDRESS
            {new Options("pid",ValueType.One)},
            {new Options("ports",ValueType.List){ AdditionalInfo="Short"} },
            {new Options("ports",ValueType.Empty,new List<Options>()
            {
                {new Options("target",ValueType.One)},
                {new Options("published",ValueType.One)},
                {new Options("protocol",ValueType.ComboBox)},
                {new Options("mode",ValueType.ComboBox)}
            }){ AdditionalInfo="Long"} },
            {new Options("restart",ValueType.ComboBox)},
            {new Options("secrets",ValueType.List){ AdditionalInfo="Short"}},
            {new Options("secrets",ValueType.Empty,ConfigNSecret){ AdditionalInfo="Long"} },
            {new Options("security_opt",ValueType.List)},
            {new Options("stop_grace_period",ValueType.One){HelpInfo = "Время в секундах(5s) или минутаз и секундах(1m30s)"} },
            {new Options("stop_signal",ValueType.One)},
            {new Options("sysctls",ValueType.ListWithValue)},
            {new Options("tmpfs",ValueType.OneOrList)},
            {new Options("ulimits",ValueType.Empty,new List<Options>()
            {
                {new Options("nproc",ValueType.One)},
                {new Options("nofile",ValueType.Empty,new List<Options>()
                {
                    {new Options("soft",ValueType.One)},
                    {new Options("hard",ValueType.One)}
                })}
            })},
            {new Options("userns_mode",ValueType.One)},
            {new Options("volumes",ValueType.List){ AdditionalInfo="Short"} },
            {new Options("volumes",ValueType.List,new List<Options>()
            {
                {new Options("type",ValueType.ComboBox){ComboBoxValues = new string[]{"volume","bind","tmpfs","npipe" } } },
                {new Options("source",ValueType.One)},
                {new Options("target",ValueType.One)},
                {new Options("read_only",ValueType.Bool)},
                {new Options("bind",ValueType.Empty,new List<Options>()
                {
                    new Options("propagation",ValueType.ComboBox){ComboBoxValues=new string[]{ "rprivate", "private", "rshared", "shared", "rslave", "slave"} }
                })},
                {new Options("volume",ValueType.Empty,new List<Options>()
                {
                    new Options("nocopy",ValueType.Bool)
                })},
                {new Options("tmpfs",ValueType.Empty,new List<Options>()
                {
                    new Options("size",ValueType.Bool){ HelpInfo ="в байтах"}
                })},
                {new Options("consistency",ValueType.ComboBox){ComboBoxValues=new string[]{ "consistent", "delegated", "cached"} }}
            }){ AdditionalInfo="Long"} },
            {new Options("user",ValueType.One)},
            {new Options("working_dir",ValueType.One)},
            {new Options("domainname",ValueType.One)},
            {new Options("hostname",ValueType.One)},
            {new Options("ipc",ValueType.One)},
            {new Options("mac_address",ValueType.One)},
            {new Options("shm_size",ValueType.One)},
            {new Options("privileged",ValueType.Bool)},
            {new Options("read_only",ValueType.Bool)},
            {new Options("stdin_open",ValueType.Bool)},
            {new Options("tty",ValueType.Bool)}
        };
        static readonly List<Options> VolumeOptions = new List<Options>() 
        {
            GlobalOptions[1],
            driver,
            name,
            external,
            driver_opts
        };
        static readonly List<Options> NetworksOptions = new List<Options>()
        {
            GlobalOptions[1],
            driver,
            {new Options("bridge",ValueType.One)},
            {new Options("overlay",ValueType.One)},
            name,
            external,
            driver_opts,
            {new Options("attachable",ValueType.Bool)},
            {new Options("config",ValueType.List,new List<Options>(){ new Options("subnet",ValueType.One)})},
            {new Options("internal",ValueType.Bool)}
        };
        static readonly List<Options> ConfigsOptions = new List<Options>()
        {
            external,
            file,
            externalName
        };
        static readonly List<Options> SecretsOptions = new List<Options>()
        {
            external,
            file,
            name
        };

        //Dictionary for service config
        public static readonly Dictionary<string, List<Options>> allConfigs = new Dictionary<string, List<Options>>(5) 
        {
            {"service",GlobalOptions},
            {"volume",VolumeOptions},
            {"networks",NetworksOptions},
            {"configs",ConfigsOptions},
            {"secrets",SecretsOptions}
        };
        //BUILD If you specify image as well as build, then Compose names the built image with the webapp and optional tag specified in image
        //MODE REPLICATED Default //If the service is replicated (which is the default), specify the number of containers that should be running at any given time.
        // Write labels for drivers your hands (ﾉ◕ヮ◕)ﾉ*:･ﾟ✧
        //Separate from networks, configs, secrets,volumes settings
    }
}