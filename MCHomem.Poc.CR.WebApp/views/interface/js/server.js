Server = {

    Configuration: {
        schema: 'http'
        , domainOrIp: 'localhost'
        , port: '53443' // 50000|53443
    }
    , setService: function (service) {
        var server = Server.Configuration.schema.concat('://')
            .concat(Server.Configuration.domainOrIp).concat(':')
            .concat(Server.Configuration.port).concat('/');
        return server + service;
    }

};