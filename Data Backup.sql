PGDMP                      |           MawadaSweis_FMS    16.2    16.2 E    8           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            9           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            :           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            ;           1262    16492    MawadaSweis_FMS    DATABASE     �   CREATE DATABASE "MawadaSweis_FMS" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Arabic_Palestinian Authority.1256';
 !   DROP DATABASE "MawadaSweis_FMS";
                postgres    false            �            1259    16540    CircleGeofence    TABLE     �   CREATE TABLE public."CircleGeofence" (
    "ID" bigint NOT NULL,
    "GeofenceID" bigint,
    "Radius" bigint,
    "Latitude" real,
    "Longitude" real
);
 $   DROP TABLE public."CircleGeofence";
       public         heap    postgres    false            �            1259    16539    CircleGeofence_ID_seq    SEQUENCE     �   CREATE SEQUENCE public."CircleGeofence_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 .   DROP SEQUENCE public."CircleGeofence_ID_seq";
       public          postgres    false    222            <           0    0    CircleGeofence_ID_seq    SEQUENCE OWNED BY     U   ALTER SEQUENCE public."CircleGeofence_ID_seq" OWNED BY public."CircleGeofence"."ID";
          public          postgres    false    221            �            1259    16594    Driver    TABLE        CREATE TABLE public."Driver" (
    "DriverID" bigint NOT NULL,
    "DriverName" character varying,
    "PhoneNumber" bigint
);
    DROP TABLE public."Driver";
       public         heap    postgres    false            �            1259    16593    Driver_DriverID_seq    SEQUENCE     ~   CREATE SEQUENCE public."Driver_DriverID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 ,   DROP SEQUENCE public."Driver_DriverID_seq";
       public          postgres    false    228            =           0    0    Driver_DriverID_seq    SEQUENCE OWNED BY     Q   ALTER SEQUENCE public."Driver_DriverID_seq" OWNED BY public."Driver"."DriverID";
          public          postgres    false    227            �            1259    16531 	   Geofences    TABLE       CREATE TABLE public."Geofences" (
    "GeofenceID" bigint NOT NULL,
    "GeofenceType" character varying,
    "AddedDate" bigint,
    "StrockColor" character varying,
    "StrockOpacity" real,
    "StrockWeight" real,
    "FillColor" character varying,
    "FillOpacity" real
);
    DROP TABLE public."Geofences";
       public         heap    postgres    false            �            1259    16530    Geofences_GeofenceID_seq    SEQUENCE     �   CREATE SEQUENCE public."Geofences_GeofenceID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 1   DROP SEQUENCE public."Geofences_GeofenceID_seq";
       public          postgres    false    220            >           0    0    Geofences_GeofenceID_seq    SEQUENCE OWNED BY     [   ALTER SEQUENCE public."Geofences_GeofenceID_seq" OWNED BY public."Geofences"."GeofenceID";
          public          postgres    false    219            �            1259    16575    PolygonGeofence    TABLE     �   CREATE TABLE public."PolygonGeofence" (
    "ID" bigint NOT NULL,
    "GeofenceID" bigint,
    "Latitude" real,
    "Longitude" real
);
 %   DROP TABLE public."PolygonGeofence";
       public         heap    postgres    false            �            1259    16574    PolygonGeofence_ID_seq    SEQUENCE     �   CREATE SEQUENCE public."PolygonGeofence_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 /   DROP SEQUENCE public."PolygonGeofence_ID_seq";
       public          postgres    false    226            ?           0    0    PolygonGeofence_ID_seq    SEQUENCE OWNED BY     W   ALTER SEQUENCE public."PolygonGeofence_ID_seq" OWNED BY public."PolygonGeofence"."ID";
          public          postgres    false    225            �            1259    16563    RectangleGeofence    TABLE     �   CREATE TABLE public."RectangleGeofence" (
    "ID" bigint NOT NULL,
    "GeofenceID" bigint,
    "North" real,
    "East" real,
    "West" real,
    "South" real
);
 '   DROP TABLE public."RectangleGeofence";
       public         heap    postgres    false            �            1259    16562    RectangleGeofence_ID_seq    SEQUENCE     �   CREATE SEQUENCE public."RectangleGeofence_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 1   DROP SEQUENCE public."RectangleGeofence_ID_seq";
       public          postgres    false    224            @           0    0    RectangleGeofence_ID_seq    SEQUENCE OWNED BY     [   ALTER SEQUENCE public."RectangleGeofence_ID_seq" OWNED BY public."RectangleGeofence"."ID";
          public          postgres    false    223            �            1259    16517    RouteHistory    TABLE        CREATE TABLE public."RouteHistory" (
    "RouteHistoryID" bigint NOT NULL,
    "VehicleDirection" integer,
    "Status" "char",
    "VehicleSpeed" character varying,
    "Address" character varying,
    "Latitude" real,
    "Longitude" real,
    "Epoch" bigint,
    "VehicleID" bigint
);
 "   DROP TABLE public."RouteHistory";
       public         heap    postgres    false            �            1259    16516    RouteHistory_RouteHistoryID_seq    SEQUENCE     �   CREATE SEQUENCE public."RouteHistory_RouteHistoryID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 8   DROP SEQUENCE public."RouteHistory_RouteHistoryID_seq";
       public          postgres    false    218            A           0    0    RouteHistory_RouteHistoryID_seq    SEQUENCE OWNED BY     i   ALTER SEQUENCE public."RouteHistory_RouteHistoryID_seq" OWNED BY public."RouteHistory"."RouteHistoryID";
          public          postgres    false    217            �            1259    16494    Vehicles    TABLE     �   CREATE TABLE public."Vehicles" (
    "VehicleNumber" bigint,
    "VehicleType" character varying,
    "VehicleID" bigint NOT NULL
);
    DROP TABLE public."Vehicles";
       public         heap    postgres    false            �            1259    16511    VehiclesInformations    TABLE     �   CREATE TABLE public."VehiclesInformations" (
    "DriverID" bigint,
    "VehicleMake" character varying,
    "VehicleModel" character varying,
    "PurchaseDate" bigint,
    "ID" bigint NOT NULL,
    "VehicleID" bigint
);
 *   DROP TABLE public."VehiclesInformations";
       public         heap    postgres    false            �            1259    16607    VehiclesInformations_ID_seq    SEQUENCE     �   CREATE SEQUENCE public."VehiclesInformations_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 4   DROP SEQUENCE public."VehiclesInformations_ID_seq";
       public          postgres    false    216            B           0    0    VehiclesInformations_ID_seq    SEQUENCE OWNED BY     a   ALTER SEQUENCE public."VehiclesInformations_ID_seq" OWNED BY public."VehiclesInformations"."ID";
          public          postgres    false    229            �            1259    16680    Vehicles_VehicleID_seq    SEQUENCE     �   CREATE SEQUENCE public."Vehicles_VehicleID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 /   DROP SEQUENCE public."Vehicles_VehicleID_seq";
       public          postgres    false    215            C           0    0    Vehicles_VehicleID_seq    SEQUENCE OWNED BY     W   ALTER SEQUENCE public."Vehicles_VehicleID_seq" OWNED BY public."Vehicles"."VehicleID";
          public          postgres    false    230            w           2604    16543    CircleGeofence ID    DEFAULT     |   ALTER TABLE ONLY public."CircleGeofence" ALTER COLUMN "ID" SET DEFAULT nextval('public."CircleGeofence_ID_seq"'::regclass);
 D   ALTER TABLE public."CircleGeofence" ALTER COLUMN "ID" DROP DEFAULT;
       public          postgres    false    222    221    222            z           2604    16597    Driver DriverID    DEFAULT     x   ALTER TABLE ONLY public."Driver" ALTER COLUMN "DriverID" SET DEFAULT nextval('public."Driver_DriverID_seq"'::regclass);
 B   ALTER TABLE public."Driver" ALTER COLUMN "DriverID" DROP DEFAULT;
       public          postgres    false    227    228    228            v           2604    16534    Geofences GeofenceID    DEFAULT     �   ALTER TABLE ONLY public."Geofences" ALTER COLUMN "GeofenceID" SET DEFAULT nextval('public."Geofences_GeofenceID_seq"'::regclass);
 G   ALTER TABLE public."Geofences" ALTER COLUMN "GeofenceID" DROP DEFAULT;
       public          postgres    false    220    219    220            y           2604    16578    PolygonGeofence ID    DEFAULT     ~   ALTER TABLE ONLY public."PolygonGeofence" ALTER COLUMN "ID" SET DEFAULT nextval('public."PolygonGeofence_ID_seq"'::regclass);
 E   ALTER TABLE public."PolygonGeofence" ALTER COLUMN "ID" DROP DEFAULT;
       public          postgres    false    226    225    226            x           2604    16566    RectangleGeofence ID    DEFAULT     �   ALTER TABLE ONLY public."RectangleGeofence" ALTER COLUMN "ID" SET DEFAULT nextval('public."RectangleGeofence_ID_seq"'::regclass);
 G   ALTER TABLE public."RectangleGeofence" ALTER COLUMN "ID" DROP DEFAULT;
       public          postgres    false    224    223    224            u           2604    16520    RouteHistory RouteHistoryID    DEFAULT     �   ALTER TABLE ONLY public."RouteHistory" ALTER COLUMN "RouteHistoryID" SET DEFAULT nextval('public."RouteHistory_RouteHistoryID_seq"'::regclass);
 N   ALTER TABLE public."RouteHistory" ALTER COLUMN "RouteHistoryID" DROP DEFAULT;
       public          postgres    false    217    218    218            s           2604    16681    Vehicles VehicleID    DEFAULT     ~   ALTER TABLE ONLY public."Vehicles" ALTER COLUMN "VehicleID" SET DEFAULT nextval('public."Vehicles_VehicleID_seq"'::regclass);
 E   ALTER TABLE public."Vehicles" ALTER COLUMN "VehicleID" DROP DEFAULT;
       public          postgres    false    230    215            t           2604    16608    VehiclesInformations ID    DEFAULT     �   ALTER TABLE ONLY public."VehiclesInformations" ALTER COLUMN "ID" SET DEFAULT nextval('public."VehiclesInformations_ID_seq"'::regclass);
 J   ALTER TABLE public."VehiclesInformations" ALTER COLUMN "ID" DROP DEFAULT;
       public          postgres    false    229    216            -          0    16540    CircleGeofence 
   TABLE DATA           a   COPY public."CircleGeofence" ("ID", "GeofenceID", "Radius", "Latitude", "Longitude") FROM stdin;
    public          postgres    false    222   xT       3          0    16594    Driver 
   TABLE DATA           K   COPY public."Driver" ("DriverID", "DriverName", "PhoneNumber") FROM stdin;
    public          postgres    false    228   U       +          0    16531 	   Geofences 
   TABLE DATA           �   COPY public."Geofences" ("GeofenceID", "GeofenceType", "AddedDate", "StrockColor", "StrockOpacity", "StrockWeight", "FillColor", "FillOpacity") FROM stdin;
    public          postgres    false    220   �U       1          0    16575    PolygonGeofence 
   TABLE DATA           X   COPY public."PolygonGeofence" ("ID", "GeofenceID", "Latitude", "Longitude") FROM stdin;
    public          postgres    false    226   �V       /          0    16563    RectangleGeofence 
   TABLE DATA           c   COPY public."RectangleGeofence" ("ID", "GeofenceID", "North", "East", "West", "South") FROM stdin;
    public          postgres    false    224   ,W       )          0    16517    RouteHistory 
   TABLE DATA           �   COPY public."RouteHistory" ("RouteHistoryID", "VehicleDirection", "Status", "VehicleSpeed", "Address", "Latitude", "Longitude", "Epoch", "VehicleID") FROM stdin;
    public          postgres    false    218   �W       &          0    16494    Vehicles 
   TABLE DATA           Q   COPY public."Vehicles" ("VehicleNumber", "VehicleType", "VehicleID") FROM stdin;
    public          postgres    false    215   �Y       '          0    16511    VehiclesInformations 
   TABLE DATA           ~   COPY public."VehiclesInformations" ("DriverID", "VehicleMake", "VehicleModel", "PurchaseDate", "ID", "VehicleID") FROM stdin;
    public          postgres    false    216   Z       D           0    0    CircleGeofence_ID_seq    SEQUENCE SET     F   SELECT pg_catalog.setval('public."CircleGeofence_ID_seq"', 10, true);
          public          postgres    false    221            E           0    0    Driver_DriverID_seq    SEQUENCE SET     D   SELECT pg_catalog.setval('public."Driver_DriverID_seq"', 12, true);
          public          postgres    false    227            F           0    0    Geofences_GeofenceID_seq    SEQUENCE SET     I   SELECT pg_catalog.setval('public."Geofences_GeofenceID_seq"', 10, true);
          public          postgres    false    219            G           0    0    PolygonGeofence_ID_seq    SEQUENCE SET     G   SELECT pg_catalog.setval('public."PolygonGeofence_ID_seq"', 10, true);
          public          postgres    false    225            H           0    0    RectangleGeofence_ID_seq    SEQUENCE SET     I   SELECT pg_catalog.setval('public."RectangleGeofence_ID_seq"', 10, true);
          public          postgres    false    223            I           0    0    RouteHistory_RouteHistoryID_seq    SEQUENCE SET     P   SELECT pg_catalog.setval('public."RouteHistory_RouteHistoryID_seq"', 13, true);
          public          postgres    false    217            J           0    0    VehiclesInformations_ID_seq    SEQUENCE SET     L   SELECT pg_catalog.setval('public."VehiclesInformations_ID_seq"', 13, true);
          public          postgres    false    229            K           0    0    Vehicles_VehicleID_seq    SEQUENCE SET     G   SELECT pg_catalog.setval('public."Vehicles_VehicleID_seq"', 14, true);
          public          postgres    false    230            �           2606    16545 "   CircleGeofence CircleGeofence_pkey 
   CONSTRAINT     f   ALTER TABLE ONLY public."CircleGeofence"
    ADD CONSTRAINT "CircleGeofence_pkey" PRIMARY KEY ("ID");
 P   ALTER TABLE ONLY public."CircleGeofence" DROP CONSTRAINT "CircleGeofence_pkey";
       public            postgres    false    222            �           2606    16712 *   VehiclesInformations Driver and Vehicle ID 
   CONSTRAINT     �   ALTER TABLE ONLY public."VehiclesInformations"
    ADD CONSTRAINT "Driver and Vehicle ID" UNIQUE ("DriverID", "VehicleID") INCLUDE ("DriverID", "VehicleID");
 X   ALTER TABLE ONLY public."VehiclesInformations" DROP CONSTRAINT "Driver and Vehicle ID";
       public            postgres    false    216    216            �           2606    16710    Driver DriverPhoneNumber 
   CONSTRAINT     x   ALTER TABLE ONLY public."Driver"
    ADD CONSTRAINT "DriverPhoneNumber" UNIQUE ("PhoneNumber") INCLUDE ("PhoneNumber");
 F   ALTER TABLE ONLY public."Driver" DROP CONSTRAINT "DriverPhoneNumber";
       public            postgres    false    228            �           2606    16601    Driver Driver_pkey 
   CONSTRAINT     \   ALTER TABLE ONLY public."Driver"
    ADD CONSTRAINT "Driver_pkey" PRIMARY KEY ("DriverID");
 @   ALTER TABLE ONLY public."Driver" DROP CONSTRAINT "Driver_pkey";
       public            postgres    false    228            �           2606    16538    Geofences Geofences_pkey 
   CONSTRAINT     d   ALTER TABLE ONLY public."Geofences"
    ADD CONSTRAINT "Geofences_pkey" PRIMARY KEY ("GeofenceID");
 F   ALTER TABLE ONLY public."Geofences" DROP CONSTRAINT "Geofences_pkey";
       public            postgres    false    220            �           2606    16580 $   PolygonGeofence PolygonGeofence_pkey 
   CONSTRAINT     h   ALTER TABLE ONLY public."PolygonGeofence"
    ADD CONSTRAINT "PolygonGeofence_pkey" PRIMARY KEY ("ID");
 R   ALTER TABLE ONLY public."PolygonGeofence" DROP CONSTRAINT "PolygonGeofence_pkey";
       public            postgres    false    226            �           2606    16568 (   RectangleGeofence RectangleGeofence_pkey 
   CONSTRAINT     l   ALTER TABLE ONLY public."RectangleGeofence"
    ADD CONSTRAINT "RectangleGeofence_pkey" PRIMARY KEY ("ID");
 V   ALTER TABLE ONLY public."RectangleGeofence" DROP CONSTRAINT "RectangleGeofence_pkey";
       public            postgres    false    224            �           2606    16524    RouteHistory RouteHistory_pkey 
   CONSTRAINT     n   ALTER TABLE ONLY public."RouteHistory"
    ADD CONSTRAINT "RouteHistory_pkey" PRIMARY KEY ("RouteHistoryID");
 L   ALTER TABLE ONLY public."RouteHistory" DROP CONSTRAINT "RouteHistory_pkey";
       public            postgres    false    218            �           2606    16615 .   VehiclesInformations VehiclesInformations_pkey 
   CONSTRAINT     r   ALTER TABLE ONLY public."VehiclesInformations"
    ADD CONSTRAINT "VehiclesInformations_pkey" PRIMARY KEY ("ID");
 \   ALTER TABLE ONLY public."VehiclesInformations" DROP CONSTRAINT "VehiclesInformations_pkey";
       public            postgres    false    216            |           2606    16688    Vehicles Vehicles_pkey 
   CONSTRAINT     a   ALTER TABLE ONLY public."Vehicles"
    ADD CONSTRAINT "Vehicles_pkey" PRIMARY KEY ("VehicleID");
 D   ALTER TABLE ONLY public."Vehicles" DROP CONSTRAINT "Vehicles_pkey";
       public            postgres    false    215            ~           2606    16708    Vehicles unique_vehicle 
   CONSTRAINT     n   ALTER TABLE ONLY public."Vehicles"
    ADD CONSTRAINT unique_vehicle UNIQUE ("VehicleNumber", "VehicleType");
 C   ALTER TABLE ONLY public."Vehicles" DROP CONSTRAINT unique_vehicle;
       public            postgres    false    215    215            �           2606    16602    VehiclesInformations DriverID    FK CONSTRAINT     �   ALTER TABLE ONLY public."VehiclesInformations"
    ADD CONSTRAINT "DriverID" FOREIGN KEY ("DriverID") REFERENCES public."Driver"("DriverID") NOT VALID;
 K   ALTER TABLE ONLY public."VehiclesInformations" DROP CONSTRAINT "DriverID";
       public          postgres    false    4752    228    216            �           2606    16546    CircleGeofence GeofenceID    FK CONSTRAINT     �   ALTER TABLE ONLY public."CircleGeofence"
    ADD CONSTRAINT "GeofenceID" FOREIGN KEY ("GeofenceID") REFERENCES public."Geofences"("GeofenceID");
 G   ALTER TABLE ONLY public."CircleGeofence" DROP CONSTRAINT "GeofenceID";
       public          postgres    false    220    4742    222            �           2606    16569    RectangleGeofence GeofenceID    FK CONSTRAINT     �   ALTER TABLE ONLY public."RectangleGeofence"
    ADD CONSTRAINT "GeofenceID" FOREIGN KEY ("GeofenceID") REFERENCES public."Geofences"("GeofenceID");
 J   ALTER TABLE ONLY public."RectangleGeofence" DROP CONSTRAINT "GeofenceID";
       public          postgres    false    220    4742    224            �           2606    16581    PolygonGeofence GeofenceID    FK CONSTRAINT     �   ALTER TABLE ONLY public."PolygonGeofence"
    ADD CONSTRAINT "GeofenceID" FOREIGN KEY ("GeofenceID") REFERENCES public."Geofences"("GeofenceID");
 H   ALTER TABLE ONLY public."PolygonGeofence" DROP CONSTRAINT "GeofenceID";
       public          postgres    false    220    4742    226            �           2606    16689    VehiclesInformations VehicleID    FK CONSTRAINT     �   ALTER TABLE ONLY public."VehiclesInformations"
    ADD CONSTRAINT "VehicleID" FOREIGN KEY ("VehicleID") REFERENCES public."Vehicles"("VehicleID") ON UPDATE CASCADE ON DELETE CASCADE NOT VALID;
 L   ALTER TABLE ONLY public."VehiclesInformations" DROP CONSTRAINT "VehicleID";
       public          postgres    false    215    4732    216            �           2606    16694    RouteHistory VehicleID    FK CONSTRAINT     �   ALTER TABLE ONLY public."RouteHistory"
    ADD CONSTRAINT "VehicleID" FOREIGN KEY ("VehicleID") REFERENCES public."Vehicles"("VehicleID") ON UPDATE CASCADE ON DELETE CASCADE NOT VALID;
 D   ALTER TABLE ONLY public."RouteHistory" DROP CONSTRAINT "VehicleID";
       public          postgres    false    218    215    4732            -   {   x�5�Q1E��g1)������1���:Q-+2�/eSRT��f�j�Q}.�*��=�q�s�Q��{n�os:�^g���Nհo�����eǣ�d(%�qU��f$�~��k�o�|�(j      3   �   x�-ν
�0����*rb�&m�.��U�.��'�H*&v��M ��s������;kH��(�J*V�齀K�JVe!XI#��=l�M���g�r�.p�O�z���Zр�� ��K=��=�1�1*�:g���a}��yKWx���`��Ėz|�t$��}��u+=C      +   �   x�U�M��0��̏�I&_���+^�/��Z)��75cZsj��fR�~�R崶�ʂC<m,h�J��?u>�n�p.a���a7�8��+�1��?o2���8��\�!S
r�~B�8���$�e3�=��~X�ɒ���b�|O9_�°����9�nѯD��w2�.��>��	���<�D
F��Ű��K�]D����
D�"1N��&^�4�D|9�pW      1   j   x�]��CAC�����O���_G� �d{���c��Vw�Kq�k-�kt���{G�O�E�cZ�����l��ƙJw�u��g�予Z#urw���LNXPy�������!�      /   �   x�-PAD1[s�>Q�����1��6BHic��������F�,3V�~�g4�|P���7%m]�S���l��{��?2&��L�1v³�xqL_���|+/2>�~}�B�h��:�9���8��U�)��/������3�,�w���:E�z[e��U����e�>c      )   �  x�]��n�0���S�B��o�^U��T`��U-��I���*O�o B�,f�|��� PXPo�Ǔx�Ӫ��z�K��xG�ڌL-�!h�@�;D��fVOq�[<"07qA��$� WB���qy?_��!�sP��\�6��o�t����JӮ��ӏ�]��}P�WY�ca��6�4����X�&���>ϯ��v��9-�u5�fSt�����6��9��r��[q`�"�f�M���uC���^����uP�(��
�|axM�6���r=�cZ��.��B����:V��2#����2t3=ǧ�2_/���s��3��("���p�8k�/+H������-n��K랶A��p4B�TFb���H�����3����9}�γ�����}�R�бf      &   �   x���� E��>�P��K�&�N]���؀!hҿ���s�L#$��I��z=���AQyT��.-��4��p����u�HV�%�Ϻ��Sm*��AS����&Ƙc�?�	C���v�MX���.@��BD
)�      '   �   x���j�0E�w��?ࢧ%m�҅)���Ѝ�EH�"٥��W
�0��yp�m����5c�Fp���er8���x�λ����K��-GǸ�٣�)�n������$�_�i�W�!ǩ��8��. H�-/�ۦ�O�
Ua	I
�XX��k�a����"+(����8��WB����b�XҰ�:�	�+Ӣ%��~˩aFWK��2cO��!�>�!�!7�0=���R�b8��ga�땈�@�K�     