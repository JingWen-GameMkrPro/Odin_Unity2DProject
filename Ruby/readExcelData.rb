#require 'rubyXL'
#require 'google/protobuf'
require 'open3'
require 'fileutils'
class Proto2Ruby
    def self.process
        argument = "--ruby_out=. --proto_path=C:/Users/aa290/Desktop/UnityProject/Odin_Unity2DProject/Ruby C:/Users/aa290/Desktop/UnityProject/Odin_Unity2DProject/Ruby/weapon_empty_attribute.proto"
        cmd = "C:/Users/aa290/Desktop/UnityProject/Odin_Unity2DProject/Proto/protoc.exe  #{argument}"
        puts "#{cmd}"
        puts "++++++++"
        puts "++++++++"
        is_success = system(cmd);
        
        if is_success
            puts "Success"
        else
            puts "Fail"
        end
    end
end

Proto2Ruby.process