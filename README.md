# NLog.RestTarget
## What
Rest API logging with NLog made easy. Generates log entries as JSON. XML is not supported yet. These entries can be sent over to RestApis. 

## What problems does this solve?
Webservice Target of NLog does not support JSON Layout https://github.com/NLog/NLog/issues/1905. It requires atleast one root parameter as below example

```
<target xsi:type="WebService">
<header name="Custom" layout="${mdlc:item=xxx}"/> <!-- Introduced in NLog 4.5 -->
<parameter name="root">
  <layout xsi:type="JsonLayout" includeAllProperties="true">
     <attribute name="Message" layout="${message}" />
  </layout>
</parameter>
</target>
```

NLog.RestTarget solves these problems. You don't require to add root parameter for JSON Layout.

## How to use it
1. Install the **NLog.RestTarget** package from NuGet
2. Add **NLog.RestTarget** to NLog extensions

```
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd">
  <extensions>
    <add assembly="NLog.RestTarget" />
  </extensions>
-----
</nlog>
```

3. Add to NLog targets

```
<targets>
  <target name="wse" xsi:type="RestService" host="http://localhost:59802/api/hello/indexer">    
    <header name="Custom" layout="${mdlc:item=xxx}"/>
    <layout xsi:type="JsonLayout" includeAllProperties="true">
      <attribute name="Level" layout="${level}" />
      <attribute name="Timestamp" layout="${longdate}" />
      <attribute name="Message" layout="${message}" />
    </layout>
  </target>
</targets>
 ````
 
```<parameter />``` is not supported. NLog.RestTarget only solves the JSON Layout problem.

## Known Issues
 
1. Supports only JSON body.
2. Supports only HTTP Post Verb.
 
