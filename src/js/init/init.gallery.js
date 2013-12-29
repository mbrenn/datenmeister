
define([],
    function () {
        // First load
        $(document).ready(function () {
            require(['../js/dejs/dejs.gallery.js'],
                function (galleryClass) {

                    $('.start').click(function () {
                        galleryConfig = {
                            galleryDom: $("#gallerydom"),
                            cacheSize: 3,
                            getImageUrl: function (image, w, h) {
                                return "../tests/img/blume" + image.id.toString() + "gross.jpeg";
                            },
                            focusImage: function (gallery, image) {
                                if (gallery === undefined) {
                                    throw "gallery is undefined";
                                }
                                $("#galleryinfo").text(gallery.title);
                                $("#imageinfo").text(image.title);
                            },
                            getImageSize: function (w, h) {
                                return {
                                    x: 800,
                                    y: 600
                                }
                            }
                        };

                        galleryData =
                            {
                                title: "Gallery",
                                images:
                                    [
                                        {
                                            title: "Frau Blume",
                                            id: 1
                                        },
                                        {
                                            title: "Frau Blume mit Herr Blume",
                                            id: 2
                                        },
                                        {
                                            title: "Frau Blume mit Sonnenbrille",
                                            id: 3
                                        },
                                        {
                                            title: "Frau Blume mit Kopfhörern",
                                            id: 4
                                        },
                                        {
                                            title: "Frau Blume mit Telefon",
                                            id: 5
                                        },
                                        {
                                            title: "Frau Blume mit Sportschuhen",
                                            id: 6
                                        },
                                    ]
                            };

                        gallery = new galleryClass.Gallery(galleryConfig);
                        gallery.show(galleryData, 3);
                    });
                });
        });
    });