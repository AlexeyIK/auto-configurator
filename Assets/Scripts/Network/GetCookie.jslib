mergeInto(LibraryManager.library, {
    
    GetCookie: function(name) 
    {
        const cookieName = UTF8ToString(name);
        var matches = document.cookie.match(new RegExp("(?:^|; )" + cookieName.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"));
        var result = matches ? decodeURIComponent(matches[1]) : "";

        var bufferSize = lengthBytesUTF8(result) + 1;
		var buffer = _malloc(bufferSize);
		stringToUTF8(result, buffer, bufferSize);
		return buffer;
    }
});