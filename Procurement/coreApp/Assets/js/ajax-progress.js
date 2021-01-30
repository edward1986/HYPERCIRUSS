var Progress = function (url, doneCallback, updateCallback, errorCallback) {
    this.url = url;
    this.doneCallback = doneCallback;
    this.updateCallback = updateCallback;
    this.errorCallback = errorCallback;

    this.source;
    
    if (!!window.EventSource) {
        var me = this;

        this.source = new EventSource(this.url);
        
        this.source.addEventListener('message', function (e) {
            var d = JSON.parse(e.data);

            if (d.IsProgressUpdate) {
                if (me.updateCallback) me.updateCallback(d);
            } else {
                me.source.close();
                if (me.doneCallback) me.doneCallback(d);
            }
            
        }, false);
        
    } else {
        if (this.errorCallback) this.errorCallback('Server-Side Events (SSE) not available');
    }
};

